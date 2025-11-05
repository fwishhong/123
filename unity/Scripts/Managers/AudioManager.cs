using UnityEngine;
using System.Collections.Generic;

namespace MindLink
{
    /// <summary>
    /// 音频管理器 - 负责BGM和SFX的播放和管理
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        #region Singleton
        private static AudioManager _instance;
        public static AudioManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("AudioManager");
                    _instance = go.AddComponent<AudioManager>();
                }
                return _instance;
            }
        }
        #endregion

        #region Audio Sources
        private AudioSource bgmSource;
        private List<AudioSource> sfxSources = new List<AudioSource>();
        private const int SFX_POOL_SIZE = 10;
        #endregion

        #region Properties
        /// <summary>
        /// BGM音量 (0-1)
        /// </summary>
        public float BGMVolume
        {
            get => bgmSource?.volume ?? 0.7f;
            set
            {
                if (bgmSource != null)
                {
                    bgmSource.volume = Mathf.Clamp01(value);
                    PlayerPrefs.SetFloat("BGMVolume", value);
                }
            }
        }

        /// <summary>
        /// SFX音量 (0-1)
        /// </summary>
        public float SFXVolume { get; set; } = 1f;

        /// <summary>
        /// 是否静音
        /// </summary>
        public bool IsMuted { get; private set; } = false;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            transform.SetParent(GameManager.Instance.transform);

            InitializeAudioSources();
            LoadSettings();
        }
        #endregion

        #region Initialization
        /// <summary>
        /// 初始化音频源
        /// </summary>
        private void InitializeAudioSources()
        {
            // BGM音频源
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
            bgmSource.playOnAwake = false;

            // SFX音频源池
            for (int i = 0; i < SFX_POOL_SIZE; i++)
            {
                AudioSource sfx = gameObject.AddComponent<AudioSource>();
                sfx.playOnAwake = false;
                sfx.loop = false;
                sfxSources.Add(sfx);
            }

            Debug.Log("[AudioManager] Initialized with BGM and SFX sources.");
        }

        /// <summary>
        /// 加载音频设置
        /// </summary>
        private void LoadSettings()
        {
            BGMVolume = PlayerPrefs.GetFloat("BGMVolume", 0.7f);
            SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
            IsMuted = PlayerPrefs.GetInt("AudioMuted", 0) == 1;
        }
        #endregion

        #region BGM Control
        /// <summary>
        /// 播放BGM
        /// </summary>
        public void PlayBGM(AudioClip clip, bool fadeIn = true)
        {
            if (clip == null)
            {
                Debug.LogWarning("[AudioManager] BGM clip is null!");
                return;
            }

            if (bgmSource.clip == clip && bgmSource.isPlaying)
            {
                return; // 已经在播放相同的BGM
            }

            if (fadeIn)
            {
                StartCoroutine(FadeOutAndChangeBGM(clip));
            }
            else
            {
                bgmSource.clip = clip;
                bgmSource.Play();
            }

            Debug.Log($"[AudioManager] Playing BGM: {clip.name}");
        }

        /// <summary>
        /// 停止BGM
        /// </summary>
        public void StopBGM(bool fadeOut = true)
        {
            if (!bgmSource.isPlaying) return;

            if (fadeOut)
            {
                StartCoroutine(FadeBGM(bgmSource.volume, 0f, 1f, () => bgmSource.Stop()));
            }
            else
            {
                bgmSource.Stop();
            }
        }

        /// <summary>
        /// 暂停BGM
        /// </summary>
        public void PauseBGM()
        {
            bgmSource.Pause();
        }

        /// <summary>
        /// 继续BGM
        /// </summary>
        public void ResumeBGM()
        {
            bgmSource.UnPause();
        }
        #endregion

        #region SFX Control
        /// <summary>
        /// 播放音效
        /// </summary>
        public void PlaySFX(AudioClip clip, float volumeScale = 1f)
        {
            if (clip == null || IsMuted) return;

            AudioSource availableSource = GetAvailableSFXSource();
            if (availableSource != null)
            {
                availableSource.volume = SFXVolume * volumeScale;
                availableSource.PlayOneShot(clip);
            }
        }

        /// <summary>
        /// 播放UI音效
        /// </summary>
        public void PlayUISound(string soundType)
        {
            // TODO: 根据soundType播放对应的UI音效
            // 例如: "click", "hover", "success", "error"
        }

        /// <summary>
        /// 获取可用的SFX音频源
        /// </summary>
        private AudioSource GetAvailableSFXSource()
        {
            foreach (var source in sfxSources)
            {
                if (!source.isPlaying)
                {
                    return source;
                }
            }

            // 如果没有可用的，返回第一个（会打断）
            return sfxSources[0];
        }
        #endregion

        #region Volume Control
        /// <summary>
        /// 设置主音量
        /// </summary>
        public void SetMasterVolume(float volume)
        {
            AudioListener.volume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat("MasterVolume", volume);
        }

        /// <summary>
        /// 设置SFX音量
        /// </summary>
        public void SetSFXVolume(float volume)
        {
            SFXVolume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat("SFXVolume", volume);
        }

        /// <summary>
        /// 静音/取消静音
        /// </summary>
        public void ToggleMute()
        {
            IsMuted = !IsMuted;
            AudioListener.volume = IsMuted ? 0f : 1f;
            PlayerPrefs.SetInt("AudioMuted", IsMuted ? 1 : 0);
        }
        #endregion

        #region Fade Effects
        /// <summary>
        /// 淡出并切换BGM
        /// </summary>
        private System.Collections.IEnumerator FadeOutAndChangeBGM(AudioClip newClip)
        {
            float startVolume = bgmSource.volume;

            // 淡出
            yield return StartCoroutine(FadeBGM(startVolume, 0f, 0.5f, null));

            // 切换
            bgmSource.clip = newClip;
            bgmSource.Play();

            // 淡入
            yield return StartCoroutine(FadeBGM(0f, startVolume, 0.5f, null));
        }

        /// <summary>
        /// BGM渐变
        /// </summary>
        private System.Collections.IEnumerator FadeBGM(float startVol, float endVol, float duration, System.Action onComplete)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                bgmSource.volume = Mathf.Lerp(startVol, endVol, elapsed / duration);
                yield return null;
            }

            bgmSource.volume = endVol;
            onComplete?.Invoke();
        }
        #endregion

        #region Debug
        public string GetDebugInfo()
        {
            return $"=== Audio Manager ===\n" +
                   $"BGM: {(bgmSource.isPlaying ? bgmSource.clip?.name : "None")}\n" +
                   $"BGM Volume: {BGMVolume:F2}\n" +
                   $"SFX Volume: {SFXVolume:F2}\n" +
                   $"Muted: {IsMuted}\n" +
                   $"Active SFX: {CountActiveSFX()}/{SFX_POOL_SIZE}";
        }

        private int CountActiveSFX()
        {
            int count = 0;
            foreach (var source in sfxSources)
            {
                if (source.isPlaying) count++;
            }
            return count;
        }
        #endregion
    }
}
