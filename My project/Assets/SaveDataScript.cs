using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveDataScript : MonoBehaviour
{
    GameObject PlayerTarget;
    PlayerScript PlayerCurrentScript;
    public string SavedSpell1;
    public string SavedSpell2;
    public string SavedSpell3;
    float RepeatSpellGrant = 0f;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        PlayerTarget = GameObject.Find("Player");
        if (PlayerTarget != null)
        {
            PlayerCurrentScript = PlayerTarget.GetComponent<PlayerScript>();
            if (RepeatSpellGrant > 0f)
            {
                RepeatSpellGrant -= Time.deltaTime;
                PlayerCurrentScript.SpellSlot1 = SavedSpell1;
                PlayerCurrentScript.SpellSlot2 = SavedSpell2;
                PlayerCurrentScript.SpellSlot3 = SavedSpell3;
            }
            if (PlayerCurrentScript.Health <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                RepeatSpellGrant = 3;
            }
            if (PlayerCurrentScript.BeatLevel == true)
            {
                SavedSpell1 = PlayerCurrentScript.SpellSlot1;
                SavedSpell2 = PlayerCurrentScript.SpellSlot2;
                SavedSpell3 = PlayerCurrentScript.SpellSlot3;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                RepeatSpellGrant = 3;
            }
        }
    }
}
