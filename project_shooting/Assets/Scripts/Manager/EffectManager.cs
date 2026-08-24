using UnityEngine;

//エフェクトを管理するクラス
public class EffectManager : SingletonMonoBehaviour<EffectManager>
{
    /// <summary>
    /// エフェクトの再生
    /// </summary>
    /// <param name="type">エフェクトの種類</param>
    /// <param name="position">エフェクトを再生する座標</param>
    public void PlayEffect(EffectType type, Vector3 position)
    {
        PlayEffect(type, position, Vector3.one);
    }

    /// <summary>
    /// スケールを指定してエフェクトの再生
    /// </summary>
    /// <param name="type">エフェクトの種類</param>
    /// <param name="position">エフェクトを再生する座標</param>
    public void PlayEffect(EffectType type, Vector3 position, Vector3 scale)
    {
        //データベースから指定されたエフェクトデータを取得
        EffectData data = DatabaseManager.Instance.effectDatabase.GetEffectData(type);

        //エフェクトデータが存在しない場合
        if(data == null)
        {
            Debug.LogWarning($"{type}のEffectDataが見つかりません");
            return;
        }

        //指定された座標にエフェクトを生成
        GameObject effect = Instantiate(data.prefab, position, Quaternion.identity);
        
        //エフェクトのスケールを設定
        effect.transform.localScale = scale;
    }
}
