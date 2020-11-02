using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent (typeof(Volume))]
public class ToDAdjustable_PostProcessing : TimeOfDayAdjustable
{
    [SerializeField] private PostProcessingSettings _postProcessingSettings = null;

    Volume _volume;
    Bloom _bloom;
    public override void OnInitialize()
    {
        _volume = GetComponent<Volume>();
        AnalyzeVolume();
    }

    public override void OnTimeOfDayValueChange(float newTime)
    {
        if (_bloom != null)
        {
            if (_postProcessingSettings.Bloom.IntensityOn)
            {
                var intensity = _postProcessingSettings.Bloom.Intensity.Evaluate(newTime);
                _bloom.intensity.value = Mathfs.Remap(intensity, 0, 1, _postProcessingSettings.Bloom.MinIntensity, _postProcessingSettings.Bloom.MaxIntensity);
            }
            if (_postProcessingSettings.Bloom.ScatterOn)
            {
                _bloom.scatter.value = Mathf.Clamp01( _postProcessingSettings.Bloom.Scatter.Evaluate(newTime) );
            }

        }
    }

    private void AnalyzeVolume()
    {
        if (_postProcessingSettings.Bloom != null)
        {
            if (_volume.profile.TryGet(out Bloom bloom)) _bloom = bloom;
            else _bloom = _volume.profile.Add<Bloom>();
        }

    }
}
