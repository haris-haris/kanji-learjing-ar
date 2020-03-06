using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GoogleARCore;
using TMPro;
using UnityEngine.UI;
public class GameScript : MonoBehaviour
{
    

    /// <summary>
        /// A prefab for visualizing an AugmentedImage.
        /// </summary>
        public ImageScript AugmentedImageVisualizerPrefab;

/// <summary>
/// The overlay containing the fit to scan user guide.
/// </summary>
public GameObject FitToScanOverlay;

private Dictionary<int, ImageScript> m_Visualizers
    = new Dictionary<int, ImageScript>();

private List<AugmentedImage> m_TempAugmentedImages = new List<AugmentedImage>();
public List<Texture> textures = new List<Texture>();
public RawImage img;
public TextMeshProUGUI scoretext;
public TextMeshProUGUI timertext;
public Button startbtn;
public static int trueScore;
int Rand;
int Lenght = 20;
int PicNumb = 0;
List<int> list = new List<int>();
public List<Image> AnswerImage = new List<Image>();
public List<Sprite> AnswerSprite = new List<Sprite>();

public static int falseScore;
    // Start is called before the first frame update
    public void Awake()
    {
        // Enable ARCore to target 60fps camera capture frame rate on supported devices.
        // Note, Application.targetFrameRate is ignored when QualitySettings.vSyncCount != 0.
        Application.targetFrameRate = 60;
        //create a list of random
        list = new List<int>(new int[Lenght]);
        for (int j = 0; j < Lenght; j++)
        {
            Rand = Random.Range(0, 19);
            if (list.Contains(Rand))
            {
                Rand = Random.Range(0, 19);
            }
            list[j] = Rand;
        }
        Time.timeScale = 0;
        img.gameObject.SetActive(false);

    }

    /// <summary>
    /// The Unity Update method.
    /// </summary>
    public void Update()
    {
        // Exit the app when the 'back' button is pressed.
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Only allow the screen to sleep when not tracking.
        if (Session.Status != SessionStatus.Tracking)
        {
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
        }
        else
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
        //initialize picture and score
        img.texture = textures[list[PicNumb]];
        scoretext.text = trueScore.ToString();
        timertext.text = falseScore.ToString();
        PlayerPrefs.SetInt("True Score", trueScore);
        PlayerPrefs.SetInt("False Score", falseScore);
        if (PicNumb == 5)
        {
            //

            gameover();
        }
        // Get updated augmented images for this frame.
        Session.GetTrackables<AugmentedImage>(
            m_TempAugmentedImages, TrackableQueryFilter.Updated);

        // Create visualizers and anchors for updated augmented images that are tracking and do
        // not previously have a visualizer. Remove visualizers for stopped images.
        foreach (var image in m_TempAugmentedImages)
        {
            ImageScript visualizer = null;
            m_Visualizers.TryGetValue(image.DatabaseIndex, out visualizer);
            if (image.TrackingState == TrackingState.Tracking && visualizer == null && image.TrackingMethod == AugmentedImageTrackingMethod.FullTracking)
            {
                // Create an anchor to ensure that ARCore keeps tracking this augmented image.
                Anchor anchor = image.CreateAnchor(image.CenterPose);
                visualizer = (ImageScript)Instantiate(
                    AugmentedImageVisualizerPrefab, anchor.transform);
                visualizer.Image = image;
                m_Visualizers.Add(image.DatabaseIndex, visualizer);
                //check if the picture match the image
                if (image.DatabaseIndex == list[PicNumb])
                {
                    trueScore++;
                    AnswerImage[PicNumb].sprite = AnswerSprite[1];
                    PicNumb++;
                }
                else if (image.DatabaseIndex != list[PicNumb])
                {
                    falseScore++;
                    AnswerImage[PicNumb].sprite = AnswerSprite[0];
                    PicNumb++;
                }
            }
            else if (image.TrackingMethod == AugmentedImageTrackingMethod.LastKnownPose && visualizer != null)
            {
                m_Visualizers.Remove(image.DatabaseIndex);
                GameObject.Destroy(visualizer.gameObject);
            }

        }

        // Show the fit-to-scan overlay if there are no images that are Tracking.
        foreach (var visualizer in m_Visualizers.Values)
        {
            if (visualizer.Image.TrackingState == TrackingState.Tracking)
            {
                FitToScanOverlay.SetActive(false);
                return;
            }
        }

        FitToScanOverlay.SetActive(true);
    }
    public void startGame()
    {
        Time.timeScale = 1;
        startbtn.gameObject.SetActive(false);
        img.gameObject.SetActive(true);
    }
    public void gameover()
    {
        Time.timeScale = 0;
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }
}
