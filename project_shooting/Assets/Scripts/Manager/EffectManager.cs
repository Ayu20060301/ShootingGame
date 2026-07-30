using UnityEngine;

public class EffectManager : SingletonMonoBehaviour<EffectManager>
{

    /// <summary>
    /// エフェクトの再生
    /// </summary>
    /// <param name="type">エフェクトの種類</param>
    /// <param name="position">再生するポジション</param>
    public void PlayEffect(EffectType type, Vector3 position)
    {
        EffectData data = DatabaseManager.Instance.effectDatabase.GetEffectData(type);

        if(data == null)
        {
            Debug.LogWarning($"{type}のEffectDataが見つかりません");
            return;
        }

        Instantiate(data.prefab, position, Quaternion.identity);
    }
}
