using UnityEngine;

//シーン遷移の基底クラス
public abstract class SceneBase : MonoBehaviour
{
    protected virtual void Awake()
    {

    }

    public virtual void OnSceneEnter()
    {
        //フェードインなど
    }

    public virtual void OnSceneExit()
    {
        //フェードアウトなど
    }

    protected abstract void Initialize(); //各シーン固有の初期化
}
