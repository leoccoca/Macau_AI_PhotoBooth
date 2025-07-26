using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoTakingPage : UIPage
{
    [SerializeField] GameObject photoReadyGO;
    [SerializeField] GameObject photoReviewGO;

    [SerializeField] Button shootBtn;
    [SerializeField] Button confirmBtn;
    // [SerializeField] Button retakeBtn;

    //
    [SerializeField] Image PoseText;
    [SerializeField] Image reviewText;

    [SerializeField] RawImage captureImage;
    [SerializeField] Transform frameImageTrans;

    Coroutine shootCountdownCr;
    Animator countdownAnimator;

    [SerializeField] Transform cameraIconTrans;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("PhotoTakingPage Start()");
        countdownAnimator = GetComponent<Animator>();

        shootBtn.onClick.AddListener(OnShootBtnClick);
        confirmBtn.onClick.AddListener(OnConfirmBtnClick);
        //retakeBtn.onClick.AddListener(OnRetakeBtnClick);

        cameraIconTrans.position += new Vector3(0, ConfigManager.Instance.clientConfig.cameraIconOffset, 0);
    }

    private void OnDestroy()
    {
        shootBtn.onClick.RemoveListener(OnShootBtnClick);
        confirmBtn.onClick.RemoveListener(OnConfirmBtnClick);
        //retakeBtn.onClick.RemoveListener(OnRetakeBtnClick);
    }

    public override void OpenPage()
    {
        base.OpenPage();

        captureImage.texture = Webcam.instance.LiveCameraTexture;

        shootBtn.gameObject.SetActive(true);
        //retakeBtn.gameObject.SetActive(false);
        confirmBtn.gameObject.SetActive(false);

        PoseText.gameObject.SetActive(true);
        reviewText.gameObject.SetActive(false);

        photoReadyGO.SetActive(true);
        photoReviewGO.SetActive(false);
    }

    void OnShootBtnClick()
    {
        SoundManager.Instance.PlaySfx(SoundFxID.buttonClick);
        GameManager.Instance.IsShowHomeBtn = false;
        shootBtn.gameObject.SetActive(false);
        countdownAnimator.SetTrigger("Play");

        if (shootCountdownCr != null)
        {
            return;
        }
        shootCountdownCr = StartCoroutine(StartCountdown(ShootPhotoCallback));
    }

    public void ShootPhotoCallback()
    {
        Debug.Log("PhotoTakingPage.ShootImage()");
        GameManager.Instance.IsShowHomeBtn = true;
        bool isRetakeAlready = GameManager.Instance.IsRetakeAlready;

      //  retakeBtn.gameObject.SetActive(!isRetakeAlready);
        confirmBtn.gameObject.SetActive(true);

        captureImage.texture = Webcam.instance.CapturePhoto();

        photoReadyGO.SetActive(false);
        photoReviewGO.SetActive(true);


        PoseText.gameObject.SetActive(false);
        reviewText.gameObject.SetActive(true);
    }

    IEnumerator StartCountdown(Action callback)
    {
        int elapsedSecond = 0;

        while (elapsedSecond < GameDefine.SELFIE_DELAY)
        {
            SoundManager.Instance.PlaySfx(SoundFxID.countdown);
            yield return new WaitForSeconds(1f);
            elapsedSecond++;
        }
        countdownAnimator.SetTrigger("Reset");
        shootCountdownCr = null;
        callback?.Invoke();
    }

    void OnConfirmBtnClick()
    {
        HideAllBtn();
        SoundManager.Instance.PlaySfx(SoundFxID.buttonClick);

        Texture2D outputTex = Webcam.instance.CapturePhoto(true);
        GameManager.Instance.AssignRandomJobID();
        GameManager.Instance.SaveSelfieAsJPEG(outputTex);
        GameManager.Instance.WriteLocalReport();
        GameManager.Instance.UploadPhotoForAIProcess();
        UIManager.Instance.Open<LoadingPage>();
    }

    void OnRetakeBtnClick()
    {
        GameManager.Instance.IsRetakeAlready = true;
        SoundManager.Instance.PlaySfx(SoundFxID.buttonClick);

        shootBtn.gameObject.SetActive(true);
      //  retakeBtn.gameObject.SetActive(false);
        confirmBtn.gameObject.SetActive(false);

        captureImage.texture = Webcam.instance.LiveCameraTexture;

        photoReadyGO.SetActive(true);
        photoReviewGO.SetActive(false);
    }

    public void HideAllBtn()
    {
        shootBtn.gameObject.SetActive(false);
      //  retakeBtn.gameObject.SetActive(false);
        confirmBtn.gameObject.SetActive(false);
    }
}
