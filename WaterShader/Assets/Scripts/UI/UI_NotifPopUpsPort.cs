using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_NotifPopUpsPort : MonoBehaviour
{
    [SerializeField] private GameObject _popUpPrefab;

    [SerializeField] private Camera _uiCamera;


    [SerializeField] private Transform _lighthousePos;
    [SerializeField] private Transform _pubPos;
    [SerializeField] private Transform _shackPos;
    [SerializeField] private Transform _boatshopPos;

    private Animation _lighthouseAnim, _pubAnim, _shackAnim, _boatshopAnim;

    void Awake() => PlaceNotifs();

    private void Start() => CheckForNewUpdatesAvaliable();


    private void OnEnable() => CheckForNewUpdatesAvaliable();


    /// <summary>
    /// Place PopUp UI over Gameobjects in Scene
    /// </summary>
    private void PlaceNotifs()
    {
        InstantiatePopUp(_lighthousePos.position, ref _lighthouseAnim);
        InstantiatePopUp(_pubPos.position, ref _pubAnim);
        InstantiatePopUp(_shackPos.position, ref _shackAnim);
        InstantiatePopUp(_boatshopPos.position, ref _boatshopAnim);
    }
    private void InstantiatePopUp(Vector3 pos, ref Animation anim)
    {
        Vector3 screenPos = Camera.main.WorldToViewportPoint(pos);
        screenPos.z = transform.position.z;
        GameObject popUp = Instantiate(_popUpPrefab, transform);
        popUp.transform.position = _uiCamera.ViewportToWorldPoint(screenPos);
        if (popUp.GetComponentInChildren<Animation>() != null) anim = popUp.GetComponentInChildren<Animation>();
    }

    /// <summary>
    /// For each Store if they have any new Updates avaliable, play notif animation
    /// </summary>
    private void CheckForNewUpdatesAvaliable()
    {
        // for each Interactive if Update Avaliable play anim else pause anim
        if (LevelManager.Instance.CheckIfPubHasUpgrades()) _pubAnim.Play();
        else _pubAnim.Stop();

        if (LevelManager.Instance.CheckIfShackHasUpgrades()) _shackAnim.Play();
        else _shackAnim.Stop();

        if (LevelManager.Instance.CheckIfBoatShackHasUpgrades()) _boatshopAnim.Play();
        else _boatshopAnim.Stop();

        if (LevelManager.Instance.CheckIfLighthouseHasUpgrades()) _lighthouseAnim.Play();
        else _lighthouseAnim.Stop();
    }
}
