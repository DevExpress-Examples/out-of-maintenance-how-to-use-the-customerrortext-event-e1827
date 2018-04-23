Imports Microsoft.VisualBasic
Imports System
Imports System.Web
Imports DevExpress.Web.ASPxScheduler
Imports DevExpress.XtraScheduler

Public Class ResourceFiller
	Public Shared Users() As String = { "Sarah Brighton", "Ryan Fischer", "Andrew Miller" }
	Public Shared Usernames() As String = { "sbrighton", "rfischer", "amiller" }

	Public Shared Sub FillResources(ByVal storage As ASPxSchedulerStorage, ByVal count As Integer)
		Dim resources As ResourceCollection = storage.Resources.Items
		storage.BeginUpdate()
		Try
			Dim cnt As Integer = Math.Min(count, Users.Length)
			For i As Integer = 1 To cnt
				resources.Add(New Resource(Usernames(i - 1), Users(i - 1)))
			Next i
		Finally
			storage.EndUpdate()
		End Try
	End Sub
End Class

#Region "#errortexthelper"
Public Class ErrorTextHelper
	Private Shared Function NewLinesToBr(ByVal text As String) As String
		text = text.Replace(Constants.vbCr, String.Empty)
		Return text.Replace(Constants.vbLf, "<br/>")
	End Function
	Public Shared Function PrepareMessage(ByVal subjectText As String, ByVal detailInfoText As String, ByVal showDetailedErrorInfo As Boolean) As String
		Dim subject As String = String.Format("{0}" & Constants.vbLf, subjectText)
		Dim detailInfo As String = String.Format("Detailed information is included below." & Constants.vbLf + Constants.vbLf & "- {0}", detailInfoText)
		If (Not showDetailedErrorInfo) Then
			detailInfo = String.Empty
		End If
		subject = NewLinesToBr(HttpUtility.HtmlEncode(subject))
		detailInfo = NewLinesToBr(HttpUtility.HtmlEncode(detailInfo))
		Return String.Format("{0},{1}|{2}{3}", subject.Length, detailInfo.Length, subject, detailInfo)
	End Function
End Class
#End Region ' #errortexthelper