Imports System.Data
Imports System.Net
Imports privacyBookAPI

Partial Class PrivacyBookAPISample
    Inherits System.Web.UI.Page

    Private Sub TestChart_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            Dim url As String = "YOUR-PRIVACYBOOK-ENDPOINT"

            'SaveFormSample(url)
            'GetTextTemplateSample(url)
            'SaveFormWithConsentsSample(url)
            'GetConsentTemplateSample(url)
            'GetConsentListSample(url)
            'GetDatatablesSample(url)
            'GetTemplatesSample(url)
            'GetFormsSample(url)
        End If
    End Sub

    Public Sub SaveFormSample(url As String)
        Dim pbk As New privacyBook(url)
        pbk.TokenID = "YOUR-APPLICATION-TOKENID"
        'pbk.ApiKey = "YOUR-LIVEMODE-APIKEY"  
        pbk.ApiKey = "YOUR-TESTMODE-APIKEY"

        Dim form_id As String = "YOUR-FORM-ID"
        Dim consent_id As String = 9999 ' Numerical Consent Template Identifier

        If pbk.LoadForm(form_id) Then

            pbk.FirstName = "Jesus"
            pbk.LastName = "Christ"
            pbk.Email = "jezahel@jc.com"

            pbk.IPAddress = "My IP address"                 ' Optional
            pbk.ProjectID = "78654"                         ' Optional
            pbk.ProjectDescription = "Demo Project"         ' Optional
            pbk.SignatureAccountID = "45"                   ' Optional
            pbk.SignatureAccount = "Giuda Ischariota"       ' Optional

            pbk.SaveConsentValue(consent_id, True)

            If pbk.SaveForm() Then
                Me.Response.Write(String.Format("<br><br>{0} Contents Saved", pbk.savedConsents.Length))
            Else
                Me.Response.Write(String.Format("<br><br>Error {0}", pbk.errorString))
            End If
        Else
            Me.Response.Write(String.Format("<br><br>Error {0}", pbk.errorString))
        End If

    End Sub

    Public Sub SaveFormWithConsentsSample(url As String)
        Dim pbk As New privacyBook(url)
        pbk.TokenID = "YOUR-APPLICATION-TOKENID"
        'pbk.ApiKey = "YOUR-LIVEMODE-APIKEY"  
        pbk.ApiKey = "YOUR-TESTMODE-APIKEY"

        Dim form_id As String = "YOUR-FORM-ID"
        Dim email As String = "support@privacybook.it"
        Dim consent_id As String = 9999 ' Numerical Consent Template Identifier

        If pbk.LoadForm(form_id, email) Then
            'Me.Response.Write(String.Format("<br><br>Text 2048:<br>{0}", pbk.GetText(2048)))
            'Me.Response.Write(String.Format("<br><br>Text 3:<br>{0}", pbk.GetText(3)))

            Me.Response.Write(String.Format("<br><br>Read Consents from givenConsents() array"))
            If pbk.GivenConsents IsNot Nothing Then
                For Each c As privacyBook.templateConsent In pbk.GivenConsents
                    Me.Response.Write(String.Format("<br><br>Consent {0}:<br>{1}", c.TemplateID, c.Consent))
                Next

                Me.Response.Write(String.Format("<br><br>Read Consent directly through GetConsent(id)"))
                Me.Response.Write(String.Format("<br><br>Consent {0}:<br>{1}", consent_id, pbk.GetConsent(consent_id)))

            End If

        Else
            Me.Response.Write(String.Format("<br><br>Error {0}", pbk.errorString))
        End If

    End Sub

    Public Sub GetTextTemplateSample(url As String)
        Dim pbk As New privacyBook(url)
        pbk.TokenID = "YOUR-APPLICATION-TOKENID"

        Dim consent_id As String = 9999 ' Numerical Consent Template Identifier
        Dim txt As String = pbk.GetTemplateText(consent_id)

        Me.Response.Write(String.Format("<br><br>Template Text {0}:<br>{1}", consent_id, txt))
        Me.Response.Write(String.Format("<br><br>Http Status code:<br>{0}", pbk.HttpDescription))
    End Sub

    Public Sub GetConsentTemplateSample(url As String)
        Dim pbk As New privacyBook(url)
        pbk.TokenID = "YOUR-APPLICATION-TOKENID"

        Dim consent_id As String = 9999 ' Numerical Consent Template Identifier
        Dim email As String = "user@demo.it"

        Me.Response.Write(String.Format("<br><br>Template Consent {0}:<br>{1}",
                                        consent_id, pbk.GetTemplateConsent(consent_id, email)))

    End Sub

    Public Sub GetConsentListSample(url As String)
        Dim pbk As New privacyBookQuery(url)
        pbk.TokenID = "YOUR-APPLICATION-TOKENID"
        pbk.TestData = 1              ' 0=live data | 1=test data
        pbk.PageNumber = 1
        pbk.PageLen = 20
        pbk.DataTableID = 9999        ' Numeric DataTable Identifier

        If pbk.GetConsentList() Then
            Me.Response.Write(String.Format("<br><br>Total Records: {0}", pbk.RecordCount))
            If pbk.RecordCount > 0 Then
                Me.Response.Write(String.Format("<br><br>Current Page: {0}/{1}", pbk.PageNumber, pbk.PageCount))
                For Each c As privacyBook.consentData In pbk.savedConsents
                    Me.Response.Write(String.Format("<br><br>{4}. DataTable: {0} TemplateID: {1} - {2} = {3}", c.DataTableID, c.TemplateID, c.Email, c.Value, c.RowNumber))
                Next
            End If
        Else
            Me.Response.Write(String.Format("<br><br>Error {0}", pbk.errorString))
        End If
        Me.Response.Write(String.Format("<br><br>Http Status code:<br>{0}", pbk.HttpDescription))

    End Sub
    Public Sub GetDatatablesSample(url As String)
        Dim pbk As New privacyBookQuery(url)
        pbk.TokenID = "YOUR-APPLICATION-TOKENID"

        If pbk.GetDatatables() Then
            If pbk.HttpStatus = HttpStatusCode.OK Then
                For Each c As privacyBook.pbDataTable In pbk.pbDatatables
                    Me.Response.Write(String.Format("<br><br>ID: {0} - {1}", c.DataTableID, c.Description))
                Next
            Else
                Me.Response.Write(String.Format("<br><br>No Records!"))
            End If
        Else
            Me.Response.Write(String.Format("<br><br>Error {0}", pbk.errorString))
        End If
        Me.Response.Write(String.Format("<br><br>Http Status code:<br>{0}", pbk.HttpDescription))

    End Sub
    Public Sub GetTemplatesSample(url As String)
        Dim pbk As New privacyBookQuery(url)
        pbk.TokenID = "YOUR-APPLICATION-TOKENID"

        If pbk.GetTemplates("it-IT") Then
            If pbk.HttpStatus = HttpStatusCode.OK Then
                For Each t As privacyBook.pbTemplate In pbk.pbTemplates
                    Me.Response.Write(String.Format("<br><br>ID: {0}, Lang: {1}, Type: {2} - {3}",
                                                    t.TemplateID, t.Language, t.TemplateType, t.Description))
                Next
            Else
                Me.Response.Write(String.Format("<br><br>No Records!"))
            End If
        Else
            Me.Response.Write(String.Format("<br><br>Error {0}", pbk.errorString))
        End If
        Me.Response.Write(String.Format("<br><br>Http Status code:<br>{0}", pbk.HttpDescription))

    End Sub
    Public Sub GetFormsSample(url As String)
        Dim pbk As New privacyBookQuery(url)
        pbk.TokenID = "YOUR-APPLICATION-TOKENID"

        If pbk.GetForms("it-IT") Then
            If pbk.HttpStatus = HttpStatusCode.OK Then
                For Each f As privacyBook.pbForm In pbk.pbForms
                    Me.Response.Write(String.Format("<br><br>ID:{0}, Lang: {1}, DataTableID: {2} - {3}",
                                                    f.FormID, f.Language, f.DataTableID, f.Description))
                Next
            Else
                Me.Response.Write(String.Format("<br><br>No Records!"))
            End If
        Else
            Me.Response.Write(String.Format("<br><br>Error {0}", pbk.errorString))
        End If
        Me.Response.Write(String.Format("<br><br>Http Status code:<br>{0}", pbk.HttpDescription))

    End Sub
End Class
