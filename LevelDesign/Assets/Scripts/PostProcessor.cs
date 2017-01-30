using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UnityEngine.PostProcessing.Utilities {

    [ExecuteInEditMode]
    [RequireComponent(typeof(PostProcessingBehaviour))]
    public class PostProcessor : MonoBehaviour {

        private PostProcessingProfile _profile;

        [SerializeField]
        public DepthOfFieldModel.Settings _dof;

        // Use this for initialization
        void OnEnable() {

            _profile = GetComponent<PostProcessingBehaviour>().profile;
            _dof = _profile.depthOfField.settings;

        }

        // Update is called once per frame
        void Update() {

            _profile.depthOfField.settings = _dof;

        }

       
        public string ReturnFocalLength()
        {
            return _dof.focalLength.ToString();
        }

        public string ReturnAperture()
        {
            return _dof.aperture.ToString();
        }

        public string ReturnFocusDistance()
        {
            return _dof.focusDistance.ToString();
        }
    }
}