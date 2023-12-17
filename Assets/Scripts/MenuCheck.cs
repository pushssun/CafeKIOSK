using UnityEngine;

public class MenuCheck : MonoBehaviour
{
    [SerializeField] private Transform _selectSpawnPoint;
    [SerializeField] private Transform _checkSpawnPoint;


    private void Start()
    {
        foreach(Transform transform in _selectSpawnPoint)
        {
            GameObject checkMenu = Instantiate(transform.gameObject,_checkSpawnPoint);
            //불필요한 버튼 false
            checkMenu.transform.GetChild(0).gameObject.SetActive(false);
            checkMenu.transform.GetChild(2).gameObject.SetActive(false);
            checkMenu.transform.GetChild(3).gameObject.SetActive(false);

        }

        
    }

}
