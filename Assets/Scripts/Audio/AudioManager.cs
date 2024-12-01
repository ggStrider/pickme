using UnityEngine;

using Data;
using Data.Observers;

namespace Audio
{
    public class AudioManager : MonoBehaviour, IAddedItem
    {
        [SerializeField] private AudioClip _itemAddedSound;
        private AudioSource _playOneShotSource;

        public void Initialize(AudioSource playOneShotSource)
        {
            _playOneShotSource = playOneShotSource;
            GameSession.Instance.SubscribeItemAdded(this);
        }
        
        public void OnItemAdded()
        {
            if(!_itemAddedSound) return;
            _playOneShotSource.PlayOneShot(_itemAddedSound);
        }
    }
}