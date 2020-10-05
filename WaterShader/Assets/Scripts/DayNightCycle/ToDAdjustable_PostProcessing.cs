using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent (typeof(Volume))]
public class ToDAdjustable_PostProcessing : TimeOfDayAdjustable
{
    Volume _volume;

    Bloom _bloom;

    [SerializeField] private PostProcessingSettings _postProcessingSettings;
    

 //   List<VolumeComponent> _activeOverrides;

    private void Awake()
    {
        _volume = GetComponent<Volume>();
        AnalyzeVolume();
    }

    public override void OnTimeOfDayValueChange(float newTime)
    {

        if (_bloom != null)
        {

        }
    }

    private void AnalyzeVolume()
    {
        Bloom bloom;
        if (_volume.profile.TryGet<Bloom>(out bloom))
        {
            _bloom = bloom;
        }
    }
}
