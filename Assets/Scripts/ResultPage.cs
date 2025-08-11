using QRCoder;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPage : UIPage
{
    [SerializeField] RawImage posterImage;
    [SerializeField] RawImage qrImage;

    [SerializeField] Button printBtn;
    [SerializeField] Button overridePrintBtn;

    float clickInterval = 1;
    int clickCount = 0;
    float timer = 0f;

    QRCodeGenerator qrGenerator;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ResultPage.Start()");
        printBtn.onClick.AddListener(OnPrintBtnClick);
        overridePrintBtn.onClick.AddListener(OnOverridePrintBtnClick);

        if (ConfigManager.Instance.clientConfig.HavePrintFunction == 0)
        {
            printBtn.gameObject.SetActive(false);
        } else
        {
            printBtn.gameObject.SetActive(true);
        }
    }

    public override void OpenPage()
    {
        // Clear QR code image
        qrImage.texture = null;

        base.OpenPage();
        LanguageController.Instance.LanguageBarActive(false);

        if (ConfigManager.Instance.clientConfig.HavePrintFunction == 1)
        {
            int todayPrintJobCount = GameManager.Instance.TodayPrintJobCount;
            int printQuotaPerDay = ConfigManager.Instance.clientConfig.printQuotaPerDay;

            if (todayPrintJobCount > printQuotaPerDay)
            {
                printBtn.gameObject.SetActive(false);
            }
            else
            {
                printBtn.gameObject.SetActive(true);
            }
        }
        GameManager.Instance.IsShowHomeBtn = true;

        posterImage.texture = Webcam.instance.CapturePoster();
        GameManager.Instance.SavePosterAsJPEG((Texture2D) posterImage.texture);
        GameManager.Instance.UploadPoster(UploadSuccess, UploadFail);
    }

    void Update()
    {
        if (clickCount > 0)
        {
            timer += Time.deltaTime;
            if (timer > clickInterval)
            {
                // Reset if time interval exceeded
                clickCount = 0;
                timer = 0f;
            }
        }
    }

    private void OnDestroy()
    {
        printBtn.onClick.RemoveListener(OnPrintBtnClick);
        overridePrintBtn.onClick.RemoveListener(OnOverridePrintBtnClick);
    }

    void OnOverridePrintBtnClick()
    {
        Debug.Log("ResultPage.OnPrintBtnClick()");
        SoundManager.Instance.PlaySfx(SoundFxID.buttonClick);
        clickCount++;
        timer = 0f; // Reset the timer with each click

        if (clickCount >= 5)
        {
            if (ConfigManager.Instance.clientConfig.HavePrintFunction == 1)
            {
                printBtn.gameObject.SetActive(true);
            }
            // DoPrint();
            clickCount = 0; // Reset the click count after triggering
        }
    }

    void DoPrint()
    {
        Debug.Log("ResultPage.DoPrint()");
        PrinterPlugin.print(posterImage.texture, false, PrinterPlugin.PrintScaleMode.FILL_PAGE);
        WebRequestManager.Instance.UpdatePrintJobRecord(UpdatePrintJobRecordSuccess, UpdatePrintJobRecordFail);
    }

    void OnPrintBtnClick()
    {
        Debug.Log("ResultPage.OnPrintBtnClick()");
        SoundManager.Instance.PlaySfx(SoundFxID.buttonClick);
        printBtn.gameObject.SetActive(false);
        DoPrint();
    }

    void UploadSuccess(string res)
    {
        Debug.Log("ResultPage.UploadSuccess reeponse: " + res);

        qrGenerator = new QRCodeGenerator();
        string link = CreateLink();
        Debug.Log("link: " + link);
        QRCodeData qRCodeData = qrGenerator.CreateQrCode(link, QRCodeGenerator.ECCLevel.Q);
        PngByteQRCode qrCodeBytes = new(qRCodeData);

        Texture2D qrTex = new(200, 200);
        qrTex.LoadImage(qrCodeBytes.GetGraphic(20));

        if (qrTex != null)
        {
            qrImage.texture = qrTex;
        }
        else
        {
            Debug.LogError("Load QR Code Fail!");
        }
    }

    void UploadFail(string res)
    {
        Debug.LogError("ResultPage.UploadFail reeponse: " + res);
    }

    void UpdatePrintJobRecordSuccess(string res)
    {
        Debug.Log("ResultPage.UpdatePrintJobRecordSuccess res: " + res);
    }

    void UpdatePrintJobRecordFail(string res)
    {
        Debug.LogError("ResultPage.UpdatePrintJobRecordFail");
    }

    string CreateLink()
    {
        string baseURL = ConfigManager.Instance.clientConfig.posterMobilePage;
        string jobID = GameManager.Instance.JobId;
        string lang = "&lang=" + LanguageController.Instance.currentLanguage;

        return baseURL + jobID + lang;
    }
}
