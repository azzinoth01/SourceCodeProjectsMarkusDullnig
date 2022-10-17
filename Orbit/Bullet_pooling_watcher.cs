using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// checks the bulletpooling list
/// </summary>
public class Bullet_pooling_watcher : MonoBehaviour
{
    /// <summary>
    /// the time a skillobject can be inactive in the bulletlist
    /// </summary>
    public float cleanUpTime;
    /// <summary>
    /// the time between each check
    /// </summary>
    public float checkTime;

    /// <summary>
    /// starts the check coroutine
    /// </summary>
    void Start() {
        StartCoroutine(cleanUpBullets(checkTime));
    }

    /// <summary>
    /// checks the billetpooling list if a skill was inactive for more than the cleanUpTime
    /// </summary>
    /// <param name="wait"> the time between the next check in seconds</param>
    /// <returns></returns>
    private IEnumerator cleanUpBullets(float wait) {
        yield return new WaitForSeconds(wait);

        float referenzTime = Time.time - cleanUpTime;
        Skill[] recycle = Globals.bulletPool.FindAll(x => x.Timestamp < referenzTime && x.gameObject.activeSelf == false).ToArray();

        foreach (Skill skill in recycle) {
            Globals.bulletPool.Remove(skill);
        }


        foreach (Skill skill in recycle) {
            Destroy(skill.gameObject);
        }

        StartCoroutine(cleanUpBullets(wait));

    }
}
