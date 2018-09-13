using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cocacola
{
    public class Chams : MonoBehaviour
    {
        internal Boolean chamsEnabled = false;

        private Material armorMaterial;
        private Material[] armorMaterials;
        private Material armorSharedMaterial;
        private Material[] armorSharedMaterials;

        private Material bodyMaterial;
        private Material[] bodyMaterials;
        private Material bodySharedMaterial;
        private Material[] bodySharedMaterials;

        public void AttachEvent()
        {
            this.GetRemoteCharacter().NetworkCharacter.Outfit.OnValueChanged += this.OutfitChanged;
        }

        public void EnableChams()
        {
            OutfitsPool.OutfitInstance _outfit = (OutfitsPool.OutfitInstance)GG.GetFieldValue(this.GetRemoteCharacter(), "_outfit");
            foreach (SkinnedMeshRenderer renderer in _outfit.ArmorMesh)
            {
                armorMaterial = renderer.material;
                armorMaterials = renderer.materials;
                armorSharedMaterial = renderer.sharedMaterial;
                armorSharedMaterials = renderer.sharedMaterials;

                int size = renderer.materials.Length;
                Material[] mats = new Material[size];
                for (int i = 0; i < size; i++)
                {
                    mats[i] = GG.silBumpedDiffuseMat;
                }
                renderer.material = GG.silBumpedDiffuseMat;
                renderer.materials = mats;
                renderer.sharedMaterial = GG.silBumpedDiffuseMat;
                renderer.sharedMaterials = mats;
            }
            foreach (SkinnedMeshRenderer renderer in _outfit.BodyMesh)
            {
                bodyMaterial = renderer.material;
                bodyMaterials = renderer.materials;
                bodySharedMaterial = renderer.sharedMaterial;
                bodySharedMaterials = renderer.sharedMaterials;

                int size = renderer.materials.Length;
                Material[] mats = new Material[size];
                for (int i = 0; i < size; i++)
                {
                    mats[i] = GG.silBumpedDiffuseMat;
                }
                renderer.material = GG.silBumpedDiffuseMat;
                renderer.materials = mats;
                renderer.sharedMaterial = GG.silBumpedDiffuseMat;
                renderer.sharedMaterials = mats;
            }
            this.chamsEnabled = true;
        }

        public void DisableChams()
        {
            OutfitsPool.OutfitInstance _outfit = (OutfitsPool.OutfitInstance)GG.GetFieldValue(this.GetRemoteCharacter(), "_outfit");
            foreach (SkinnedMeshRenderer renderer in _outfit.ArmorMesh)
            {
                int size = renderer.materials.Length;
                Material[] mats = new Material[size];
                for (int i = 0; i < size; i++)
                {
                    mats[i] = GG.silBumpedDiffuseMat;
                }
                renderer.material = armorMaterial;
                renderer.materials = armorMaterials;
                renderer.sharedMaterial = armorSharedMaterial;
                renderer.sharedMaterials = armorSharedMaterials;
            }
            foreach (SkinnedMeshRenderer renderer in _outfit.BodyMesh)
            {
                int size = renderer.materials.Length;
                Material[] mats = new Material[size];
                for (int i = 0; i < size; i++)
                {
                    mats[i] = GG.silBumpedDiffuseMat;
                }
                renderer.material = bodyMaterial;
                renderer.materials = bodyMaterials;
                renderer.sharedMaterial = bodySharedMaterial;
                renderer.sharedMaterials = bodySharedMaterials;
            }
            this.chamsEnabled = false;
        }

        private void OutfitChanged(short id)
        {
            if(this.chamsEnabled)
            {
                this.EnableChams();
            }
        }

        private RemoteCharacter GetRemoteCharacter()
        {
            return this.gameObject.GetComponent<RemoteCharacter>();
        }
    }
}
