using UnityEngine;

public class Timer : MonoBehaviour
{
    float secondLimit;
    private float timercount;

    /// <summary>
    /// You would use this function to add a timer you can ask for variable seconds passed.
    /// <br />
    /// This should be used in conjunction with SecondsPassed passing in the variable seconds expected.
    /// </summary>
    /// <param name="go"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    public static Timer CreateComponent(GameObject go)
    {
        Timer timer = go.AddComponent<Timer>();
        return timer;
    }

    /// <summary>
    /// You would use this function to add a timer which would be expected to do something every X seconds.
    /// <br />
    /// This should be used in conjunction with LimitReached to determine if x seconds have passed.
    /// </summary>
    /// <param name="go"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    public static Timer CreateComponent(GameObject go, float limit)
    {
        Timer timer = go.AddComponent<Timer>();
        timer.secondLimit = limit;

        return timer;
    }


    void Update()
    {
        timercount += Time.deltaTime;
    }

    public void Reset()
    {
        timercount = 0;
    }

    /// <summary>
    /// If the timer has been running for more than the seconds passed the true and reset else false. 
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    public bool SecondsPassed(float seconds)
    {
        if (timercount >= seconds)
        {
            timercount = 0f;
            return true;
        }

        return false;
    }

    /// <summary>
    /// If the timer has been running for more than the limit requested on initialisation then true and reset else false. 
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    public bool LimitReached()
    {
        if (timercount >= secondLimit)
        {
            timercount = 0f;
            return true;
        }

        return false;
    }
}
