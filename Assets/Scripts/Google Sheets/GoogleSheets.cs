using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

public class GoogleSheets : MonoBehaviour
{
    [SerializeField] private string[] _scopes = { SheetsService.Scope.Spreadsheets };

    [SerializeField] private string _credentialsPath = "GoogleSheet/Credentials";
    [SerializeField] private string _credentialsFile = "credentials.json";

    private bool Connected = false;

    [ContextMenu("Connect")]
    public async void Connect()
    {
        Debug.Log("Connection loading...");
        await ConnectAsync();
        Debug.Log("Connect !");
    }

    public async Task ConnectAsync()
    {
        // Create path
        string credentialsDirectoryPath = Path.Combine(Application.streamingAssetsPath, _credentialsPath);
        string credentialsPath = Path.Combine(credentialsDirectoryPath, _credentialsFile);
        if (!Directory.Exists(credentialsDirectoryPath))
        {
            Directory.CreateDirectory(credentialsDirectoryPath);
        }

        UserCredential credential;
        GoogleClientSecrets secrets;

        Debug.Log("Reading credentials...");
        // Create token
        using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
        {
            secrets = await GoogleClientSecrets.FromStreamAsync(stream);
        }

        string tokenPath = Path.Combine(Application.persistentDataPath, _credentialsPath);

        Debug.Log("Open connection");
        // Open Connection
        credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            secrets.Secrets,
            _scopes,
            "user",
            CancellationToken.None,
            new FileDataStore(tokenPath, true));

        Debug.Log("Credential saved to : " + tokenPath);
        // Create Google Sheets API service
        var _service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential
        });
        Connected = true;
    }

    /*public void Request(SheetsService service)
    { 
        // Define request parameters.
        String spreadsheetId = "1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms";
        String range = "Class Data!A2:E";
        SpreadsheetsResource.ValuesResource.GetRequest request =
                service.Spreadsheets.Values.Get(spreadsheetId, range);

        // Prints the names and majors of students in a sample spreadsheet:
        // https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
        ValueRange response = request.Execute();
        IList<IList<object>> values = response.Values;
        if (values != null && values.Count > 0)
        {
            Debug.Log("Name, Major");
            foreach (var row in values)
            {
                // Print columns A and E, which correspond to indices 0 and 4.
                Debug.Log("{0}, {1}" + row[0] + row[4]);
            }
        }
        else
        {
            Debug.Log("No data found.");
        }
        Console.Read();
    }*/
}
