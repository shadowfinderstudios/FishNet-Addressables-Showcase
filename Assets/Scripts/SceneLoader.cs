using UnityEngine;
using FishNet.Object;
using FishNet.Managing.Scened;
using FishNet.Object.Synchronizing;

public class SceneLoader : NetworkBehaviour
{
    [SerializeField] string _sceneName;

    readonly SyncVar<bool> _areaEntered = new(false);

    //[ServerRpc] void LoadGlobalSceneServerRpc() => LoadGlobalScene(_sceneName);

    void LoadConnectionScene(string name, GameObject player)
    {
        var slud = new SceneLookupData() { Handle = 0, Name = name };
        var nob = player.GetComponent<NetworkObject>();
        var sld = new SceneLoadData()
        {
            SceneLookupDatas = new SceneLookupData[] { slud },
            MovedNetworkObjects = new NetworkObject[] { nob },
            Options = new LoadOptions()
            {
                AutomaticallyUnload = true,
                AllowStacking = false,
                Addressables = true
            }
        };
        base.SceneManager.LoadConnectionScenes(nob.Owner, sld);
    }

    void LoadGlobalScene(string name, GameObject player)
    {
        var slud = new SceneLookupData() { Handle = 0, Name = name };
        var nob = player.GetComponent<NetworkObject>();
        var sld = new SceneLoadData()
        {
            SceneLookupDatas = new SceneLookupData[] { slud },
            MovedNetworkObjects = new NetworkObject[] { nob },
            Options = new LoadOptions()
            {
                AllowStacking = false,
                Addressables = true
            }
        };
        base.SceneManager.LoadGlobalScenes(sld);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!_areaEntered.Value && other.CompareTag("Player"))
        {
            _areaEntered.Value = true;
            Debug.Log("Player entered area");

            //if (other.gameObject.GetComponent<NetworkObject>().IsServerInitialized)
            //{
            //    Debug.Log("On server");
                LoadGlobalScene(_sceneName, other.gameObject);
            //}
            //else
            //{
            //    Debug.Log("On Client");
            //    LoadGlobalSceneServerRpc();
            //}
        }
    }
}