using UnityEngine;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    public GameObject planeCrashCutscene;
    public GameObject islandLandingCutscene;
    public GameObject finalBossCutscene;
    public GameObject victoryCutscene;

    public IEnumerator PlayPlaneCrashCutscene()
    {
        if (planeCrashCutscene != null) planeCrashCutscene.SetActive(true);
        yield return new WaitForSeconds(5f);
        if (planeCrashCutscene != null) planeCrashCutscene.SetActive(false);
    }

    public IEnumerator PlayIslandLandingCutscene()
    {
        if (islandLandingCutscene != null) islandLandingCutscene.SetActive(true);
        yield return new WaitForSeconds(5f);
        if (islandLandingCutscene != null) islandLandingCutscene.SetActive(false);
    }

    public IEnumerator PlayFinalBossIntroCutscene()
    {
        if (finalBossCutscene != null) finalBossCutscene.SetActive(true);
        yield return new WaitForSeconds(5f);
        if (finalBossCutscene != null) finalBossCutscene.SetActive(false);
    }

    public IEnumerator PlayVictoryCutscene()
    {
        if (victoryCutscene != null) victoryCutscene.SetActive(true);
        yield return new WaitForSeconds(7f);
        if (victoryCutscene != null) victoryCutscene.SetActive(false);
    }
}