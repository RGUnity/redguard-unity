using UnityEngine;

public class BlendShapeAnimator : MonoBehaviour
{
    SkinnedMeshRenderer skinnedMeshRenderer;
    int frameCount;
    int currentFrame;
    float frameTimer;
    bool playing;

    // Frame 0 = T-pose, frame 1 = zero-delta (rebased base pose), skip both
    int firstFrame;

    public void Initialize(SkinnedMeshRenderer smr)
    {
        skinnedMeshRenderer = smr;
        frameCount = smr.sharedMesh.blendShapeCount;
        firstFrame = frameCount > 2 ? 2 : 0;
        currentFrame = firstFrame;
        frameTimer = AnimData.FRAMETIME_VAL;
        playing = false;
    }

    public void Play()
    {
        if (frameCount > firstFrame)
        {
            playing = true;
            currentFrame = firstFrame;
            frameTimer = AnimData.FRAMETIME_VAL;
            ResetWeights();
            skinnedMeshRenderer.SetBlendShapeWeight(currentFrame, 100.0f);
        }
    }

    public void Stop()
    {
        playing = false;
        ResetWeights();
    }

    void Update()
    {
        if (!playing || frameCount <= firstFrame)
            return;

        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0)
        {
            frameTimer += AnimData.FRAMETIME_VAL;

            skinnedMeshRenderer.SetBlendShapeWeight(currentFrame, 0.0f);
            currentFrame++;
            if (currentFrame >= frameCount)
                currentFrame = firstFrame;
            skinnedMeshRenderer.SetBlendShapeWeight(currentFrame, 100.0f);
        }
    }

    void ResetWeights()
    {
        for (int i = 0; i < frameCount; i++)
            skinnedMeshRenderer.SetBlendShapeWeight(i, 0.0f);
    }
}
