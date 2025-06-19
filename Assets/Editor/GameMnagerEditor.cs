using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GameManager gm = (GameManager)target;
        WeaponData data = gm.currentWeapon;

        if (data != null)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Weapon Data Preview", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Name", data.weaponName);
            EditorGUILayout.LabelField("Attack Cooldown", data.attackCooldown.ToString());
            EditorGUILayout.LabelField("Bullet Count", data.bulletCount.ToString());
            EditorGUILayout.LabelField("Spread Angle", data.spreadAngle.ToString());
            EditorGUILayout.LabelField("Bullet Speed", data.bulletSpeed.ToString());
            EditorGUILayout.LabelField("Bullet Range", data.bulletRange.ToString());
            EditorGUILayout.LabelField("Bullet Damage", data.bulletDamage.ToString());
        }
    }
}
