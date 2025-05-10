using UnityEngine;
using System.Collections;

namespace AzureNature
{
    public class FootstepController : MonoBehaviour
    {
        public enum Surface { Dirt, Grass, Water }

        [Header("Audio Sources")]
        public AudioSource footstepAudioSource;
        public AudioSource eventAudioSource;

        [Header("Audio Clips")]
        public AudioClip[] dirtSteps;
        public AudioClip[] grassSteps;
        public AudioClip[] waterSteps;

        [Header("Event Clips")]
        public AudioClip bushClip;
        public AudioClip jumpClip;

        [Header("Step Settings")]
        public float stepRate = 0.5f;
        public float runStepMultiplier = 0.6f;

        private CharacterController characterController;
        private MoveController moveController;
        private Terrain terrain;
        private TerrainData terrainData;
        private bool isWalking;
        private bool isInWater;
        private float nextStep;
        private Surface currentSurface = Surface.Dirt;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            moveController = GetComponent<MoveController>();
            terrain = Terrain.activeTerrain;
            terrainData = terrain.terrainData;

            bushClip.LoadAudioData();
            jumpClip.LoadAudioData();
            
            for (int i = 0; i < dirtSteps.Length; i++)
            {
                dirtSteps[i].LoadAudioData();
            }

            for (int i = 0; i < grassSteps.Length; i++)
            {
                grassSteps[i].LoadAudioData();
            }
        }

        private void Update()
        {
            isWalking = (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && characterController.isGrounded;

            if (isWalking && Time.time > nextStep)
            {
                float stepDelay = stepRate;

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    stepDelay *= runStepMultiplier;
                }

                if (!isInWater)
                {
                    currentSurface = GetTerrainTextureAtPosition();
                }
                else
                {
                    currentSurface = Surface.Water;
                }

                PlayFootstepSound(currentSurface);

                nextStep = Time.time + stepDelay;
            }

            if (Input.GetButtonDown("Jump") && characterController.isGrounded)
            {
                footstepAudioSource.PlayOneShot(jumpClip);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Water"))
            {
                isInWater = true;
            }

            if (other.CompareTag("Bush"))
            {
                eventAudioSource.PlayOneShot(bushClip);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Water"))
            {
                isInWater = false;
            }
        }

        private Surface GetTerrainTextureAtPosition()
        {
            Vector3 terrainPosition = transform.position - terrain.transform.position;

            float normX = terrainPosition.x / terrainData.size.x;
            float normZ = terrainPosition.z / terrainData.size.z;

            float[,,] alphamaps = terrainData.GetAlphamaps(
                Mathf.FloorToInt(normX * terrainData.alphamapWidth),
                Mathf.FloorToInt(normZ * terrainData.alphamapHeight),
                1, 1);

            int dirtTextureIndex = 1;
            int grassTextureIndex = 0;

            if (alphamaps[0, 0, grassTextureIndex] > alphamaps[0, 0, dirtTextureIndex])
            {
                return Surface.Grass;
            }
            else
            {
                return Surface.Dirt;
            }
        }

        private void PlayFootstepSound(Surface surface)
        {
            AudioClip clip = null;

            switch (surface)
            {
                case Surface.Dirt:
                    clip = dirtSteps[Random.Range(0, dirtSteps.Length)];
                    break;
                case Surface.Grass:
                    clip = grassSteps[Random.Range(0, grassSteps.Length)];
                    break;
                case Surface.Water:
                    clip = waterSteps[Random.Range(0, waterSteps.Length)];
                    break;
            }

            if (clip != null)
            {
                footstepAudioSource.PlayOneShot(clip);
            }
        }
    }
}