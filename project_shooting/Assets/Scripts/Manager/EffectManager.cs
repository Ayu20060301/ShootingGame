using UnityEngine;

public class EffectManager : SingletonMonoBehaviour<EffectManager>
{
    /// <summary>
    /// エフェクトの再生
    /// </summary>
    /// <param name="type">エフェクトの種類</param>
    /// <param name="position">指定の座標</param>
    public void PlayEffect(EffectType type, Vector3 position)
    {
        PlayEffect(type, position, Vector3.one);
    }

    /// <summary>
    /// スケールを指定してエフェクトの再生
    /// </summary>
    /// <param name="type">エフェクトの種類</param>
    /// <param name="position">再生するポジション</param>
    public void PlayEffect(EffectType type, Vector3 position, Vector3 scale)
    {
        EffectData data = DatabaseManager.Instance.effectDatabase.GetEffectData(type);

        if(data == null)
        {
            Debug.LogWarning($"{type}のEffectDataが見つかりません");
            return;
        }

        GameObject effect = Instantiate(data.prefab, position, Quaternion.identity);
        effect.transform.localScale = scale;
    }
}
