using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Apis.Sheets;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using System.IO;
using System.Threading;
using Google.Apis.Services;
using System;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Data = Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json;
using UnityEngine.UI;

public class GoogleAPI : MonoBehaviour
{
    const string p12PathFromAsset = "Plugins/Quickstart-e6529aa9fcff.p12";
    const string spreadsheetId = "1R8Mq61-oIhcqrAf0Y8gkYZDv3f9Av9jwL8Nid8VXOVk";
    const string sheetNameAndRange = "DEMO!A2:F";
    const string serviceAccountEmail = "unity-945@quickstart-1569312874623.iam.gserviceaccount.com";
    static SheetsService service;

    public InputField[] inputFields;

    void Start()
    {
        SyncData();

        //while (true)
        //{
        //    yield return StartCoroutine(Sync());
        //}
    }

    IEnumerator Sync()
    {
        yield return new WaitForSeconds(1);
        SyncData();        
    }

    public void SyncData()
    {
        var certificate = new X509Certificate2(Application.dataPath + Path.DirectorySeparatorChar + p12PathFromAsset, "notasecret", X509KeyStorageFlags.Exportable);

        ServiceAccountCredential credential = new ServiceAccountCredential(
           new ServiceAccountCredential.Initializer(serviceAccountEmail)
           {
               Scopes = new[] { SheetsService.Scope.Spreadsheets }
               /*
                Without this scope, it will :
                GoogleApiException: Google.Apis.Requests.RequestError
                Request had invalid authentication credentials. Expected OAuth 2 access token, login cookie or other valid authentication credential.
                lol..
                */
           }.FromCertificate(certificate));


        service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
        });

        SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, sheetNameAndRange);        

        StringBuilder sb = new StringBuilder();

        ValueRange response = request.Execute();
        IList<IList<object>> values = response.Values;
        if (values != null && values.Count > 0)
        {
            foreach (IList<object> row in values)
            {
                foreach (object cell in row)
                {
                    sb.Append(cell.ToString() + " ");
                }

                //Concat the whole row
                Debug.Log(sb.ToString());
                sb.Clear();
            }
        }
        else
        {
            Debug.Log("No data found.");
        }
    }

    public void UpdateRow()
    {
        // Specifying Column Range for reading...
        var range = "DEMO!A2:F";
        var valueRange = new ValueRange();

        // Data
        var oblist = new List<object>() { inputFields[0].text, inputFields[1].text, inputFields[2].text, inputFields[3].text, inputFields[4].text };
        valueRange.Values = new List<IList<object>> { oblist };

        // Append the above record...
        var appendRequest = service.Spreadsheets.Values.Append(valueRange, spreadsheetId, range);
        appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
        //var appendReponse = appendRequest.Execute();// just for debug

        SyncData();
    }
}