using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_NotifPopUpsPort : MonoBehaviour
{
    [SerializeField] private GameObject _popUpPrefab;

    [SerializeField] private Camera _uiCamera;

    private Transform[] _interactables;
    private readonly List<Animation> _anims = new List<Animation>();

    [SerializeField] private Transform _lighthousePos;
    [SerializeField] private Transform _pubPos;
    [SerializeField] private Transform _shackPos;
    [SerializeField] private Transform _boatshopPos;

    private Animation _lighthouseAnim, _pubAnim, _shackAnim, _boatshopAnim;

    void Start()
    {
        //_interactables = GetSceneInteractives();
        //if (_interactables != null)
        //{
        //    PlaceNotifPopUpsOverInteractives();
        //    CheckForNewUpdatesAvaliable();
        //}

        PlaceNotifs();
        CheckForNewUpdatesAvaliable();
    }

    private void GetAnimsForInteractives()
    {

    }

    /// <summary>
    /// Collects all Children in Container tagged Interactives
    /// </summary>
    /// <returns></returns>
    private Transform[] GetSceneInteractives()
    {
        GameObject parent = GameObject.FindGameObjectWithTag("Interactives");
        List<Transform> children = new List<Transform>();
        for (int i = 0; i < parent.transform.childCount; i++)
        {           
            if (parent.transform.GetChild(i).Find("NotifPos")) children.Add(parent.transform.GetChild(i).Find("NotifPos"));
        }
        Transform[] fetched = children.ToArray();
        return fetched;
    }

    /// <summary>
    /// Place UI Elements over Interactive Models
    /// </summary>
    private void PlaceNotifPopUpsOverInteractives()
    {
        for (int i = 0; i < _interactables.Length; i++)
        {
            Vector3 screenPos = Camera.main.WorldToViewportPoint(_interactables[i].position);
            screenPos.z = transform.position.z;
            GameObject popUp = Instantiate(_popUpPrefab, transform);
            popUp.transform.position = _uiCamera.ViewportToWorldPoint(screenPos);
            if (popUp.GetComponentInChildren<Animation>() != null) _anims.Add(popUp.GetComponentInChildren<Animation>());
        }
    }

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
        if (LevelManager.Instance.CheckIfPubHasUpgrades())
        {
            // this is true if pub has upgrades
            _pubAnim.Play();

        }
        else _pubAnim.Stop();

        if (LevelManager.Instance.CheckIfShackHasUpgrades())
        {
            // this is true if shack has upgrades
            _shackAnim.Play();
        }
        else _shackAnim.Stop();

        if (LevelManager.Instance.CheckIfBoatShackHasUpgrades())
        {
            // this is true if boat shack has upgrades
            _boatshopAnim.Play();
        }
        else _boatshopAnim.Stop();

        if (LevelManager.Instance.CheckIfLighthouseHasUpgrades())
        {
            // this is true if lighthouse has upgrades
            _lighthouseAnim.Play();
        }
        else _lighthouseAnim.Stop();
        //╰(*°▽°*)╯)
    }
}
