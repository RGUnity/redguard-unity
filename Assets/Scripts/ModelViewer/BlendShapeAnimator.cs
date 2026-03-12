using UnityEngine;

public class BlendShapeAnimator : MonoBehaviour
{
    SkinnedMeshRenderer skinnedMeshRenderer;
    int frameCount;
    int currentFrame;
    float frameTimer;
    bool playing;

    public void Initialize(SkinnedMeshRenderer smr)
    {
        skinnedMeshRenderer = smr;
        frameCount = smr.sharedMesh.blendShapeCount;
        currentFrame = 0;
        frameTimer = AnimData.FRAMETIME_VAL;
        playing = false;
    }

    public void Play()
    {
        if (frameCount > 0)
        {
            playing = true;
            currentFrame = 0;
            frameTimer = AnimData.FRAMETIME_VAL;
            ResetWeights();
            skinnedMeshRenderer.SetBlendShapeWeight(0, 100.0f);
        }
    }

    public void Stop()
    {
        playing = false;
        ResetWeights();
    }

    void Update()
    {
        if (!playing || frameCount == 0)
            return;

        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0)
        {
            frameTimer += AnimData.FRAMETIME_VAL;

            skinnedMeshRenderer.SetBlendShapeWeight(currentFrame, 0.0f);
            currentFrame = (currentFrame + 1) % frameCount;
            skinnedMeshRenderer.SetBlendShapeWeight(currentFrame, 100.0f);
        }
    }

    void ResetWeights()
    {
        for (int i = 0; i < frameCount; i++)
            skinnedMeshRenderer.SetBlendShapeWeight(i, 0.0f);
    }
}
