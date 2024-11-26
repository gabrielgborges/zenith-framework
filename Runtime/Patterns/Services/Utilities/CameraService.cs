using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraService : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] Transform _cameraTransform;
    [SerializeField] private int _maxZoom;
    [SerializeField] private int _minZoom;
    [SerializeField] private int _limitZoom;

    private List<Transform> _players = new List<Transform>();
    [SerializeField] private Vector3 _positionOffset;

    private IEventService _eventService;

    private async void Awake()
    {
        _eventService = await ServiceLocator.GetService<IEventService>();
        _eventService.AddListener<OnSetUpPlayerEvent>(FollowMember, GetHashCode());
    }

    private void LateUpdate()
    {
        if (_players.Count > 0)
        {
            FollowGroup();
            Zoom();
        }
    }

    private void OnDestroy()
    {
        _eventService.RemoveListener<OnSetUpPlayerEvent>(GetHashCode());
    }

    private void Zoom()
    {
        float newZoom = Mathf.Lerp(_minZoom, _maxZoom, GreatestDistance() / _limitZoom);
        _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, newZoom, Time.deltaTime);
    }

    private void FollowGroup()
    {
        Bounds bounds = new Bounds(_players[0].position, Vector3.zero);

        foreach (var player in _players)
        {
            bounds.Encapsulate(player.position);
        }

        Vector3 centerPoint = bounds.center;

        _cameraTransform.position = _positionOffset + centerPoint;
    }

    private float GreatestDistance()
    {
        Bounds bounds = new Bounds(_players[0].position, Vector3.zero);

        foreach (var player in _players)
        {
            bounds.Encapsulate(player.position);
        }

        return bounds.size.x;
    }

    private void FollowMember(OnSetUpPlayerEvent gameEvent)
    {
        _players.Add(gameEvent.CombatentModule.gameObject.transform.GetChild(0));
    }
}
