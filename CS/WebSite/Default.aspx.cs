using System;
using System.Web.UI.WebControls;
#region #usings
using DevExpress.Web.ASPxScheduler;
using DevExpress.XtraScheduler;
#endregion #usings

public partial class _Default : System.Web.UI.Page 
{
    ASPxSchedulerStorage Storage { get { return ASPxScheduler1.Storage; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        SetupMappings();
        ResourceFiller.FillResources(this.ASPxScheduler1.Storage, 3);

        ASPxScheduler1.AppointmentDataSource = appointmentDataSource;
        ASPxScheduler1.DataBind();

        if(!Page.IsCallback) {
            ASPxScheduler1.Start = DateTime.Today;
            ASPxScheduler1.GroupType = SchedulerGroupType.Resource;
            AddTestAppointment();
        }

    }


    void SetupMappings() {
        ASPxAppointmentMappingInfo mappings = Storage.Appointments.Mappings;
        Storage.BeginUpdate();
        try {
            mappings.AppointmentId = "Id";
            mappings.Start = "StartTime";
            mappings.End = "EndTime";
            mappings.Subject = "Subject";
            mappings.AllDay = "AllDay";
            mappings.Description = "Description";
            mappings.Label = "Label";
            mappings.Location = "Location";
            mappings.RecurrenceInfo = "RecurrenceInfo";
            mappings.ReminderInfo = "ReminderInfo";
            mappings.ResourceId = "OwnerId";
            mappings.Status = "Status";
            mappings.Type = "EventType";
        }
        finally {
            Storage.EndUpdate();
        }
    }

    protected void appointmentsDataSource_ObjectCreated(object sender, ObjectDataSourceEventArgs e) {
        e.ObjectInstance = new CustomEventDataSource(GetCustomEvents());
    }
    CustomEventList GetCustomEvents() {
        CustomEventList events = Session["ListBoundModeObjects"] as CustomEventList;
        if(events == null) {
            events = new CustomEventList();
            Session["ListBoundModeObjects"] = events;
        }
        return events;
    }

    protected void ASPxScheduler1_AppointmentInserting(object sender, PersistentObjectCancelEventArgs e) {
        SetAppointmentId(sender, e);
    }

    void SetAppointmentId(object sender, PersistentObjectCancelEventArgs e) {
        ASPxSchedulerStorage storage = (ASPxSchedulerStorage)sender;
        Appointment apt = (Appointment)e.Object;
        storage.SetAppointmentId(apt, apt.GetHashCode());
    }
    private void AddTestAppointment() {
        ASPxScheduler1.Storage.Appointments.Clear();
        Appointment apt = ASPxScheduler1.Storage.CreateAppointment(DevExpress.XtraScheduler.AppointmentType.Normal);
        apt.BeginUpdate();
        apt.Start = DateTime.Now;
        apt.End = apt.Start.AddMinutes(90);
        apt.ResourceId = "sbrighton";
        apt.Subject = "Drag or Resize me!";
        apt.Description = "Modify this appointment to observe a custom error text displayed";
        apt.EndUpdate();
        ASPxScheduler1.Storage.Appointments.Add(apt);
    }
    protected void ASPxScheduler1_AppointmentsChanged(object sender, PersistentObjectsEventArgs e) {
        throw new ApplicationException("An unhanled exception occurred for some unknown reason");
    }
    #region #customerrortext
    protected void ASPxScheduler1_CustomErrorText(object handler, ASPxSchedulerCustomErrorTextEventArgs e) {
        e.ErrorText = ErrorTextHelper.PrepareMessage("Please contact Technical Support", e.Exception.Message, true);       
    }
    #endregion #customerrortext

}
