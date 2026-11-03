using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public struct TerrainType
{
    public float friction;
    public float restitution;
    public List<Surface> surfaces;
}

public class AA2Params : MonoBehaviour
{
    public static AA2Params instance;

    public float initialForce;
    public TerrainType grass, rock, sand;

    [Header("Ball")]
    public Body ball;
    public TMP_InputField initialForceInput;
    public TMP_InputField massInput;

    [Header("Grass")]
    public TMP_InputField grassFrictionInput;
    public TMP_InputField grassRestitutionInput;

    [Header("Rock")]
    public TMP_InputField rockFrictionInput;
    public TMP_InputField rockRestitutionInput;

    [Header("Sand")]
    public TMP_InputField sandFrictionInput;
    public TMP_InputField sandRestitutionInput;

    private float defaultInitialForce;
    private float defaultMass;
    private TerrainType defaultGrass, defaultRock, defaultSand;

    private void Awake()
    {
        instance = this;
        defaultInitialForce = initialForce;
        defaultGrass = grass;
        defaultRock = rock;
        defaultSand = sand;
    }

    private void Start()
    {
        if (ball != null) defaultMass = ball.mass;

        ApplyTerrain(grass);
        ApplyTerrain(rock);
        ApplyTerrain(sand);

        if (initialForceInput != null) {
            initialForceInput.text = initialForce.ToString(); 
            initialForceInput.onEndEdit.AddListener(SetInitialForce); 
        }
        if (massInput != null && ball != null) {
            massInput.text = ball.mass.ToString(); 
            massInput.onEndEdit.AddListener(SetMass); 
        }

        if (grassFrictionInput != null) {
            grassFrictionInput.text = grass.friction.ToString(); 
            grassFrictionInput.onEndEdit.AddListener(SetGrassFriction); 
        }
        if (grassRestitutionInput != null) { 
            grassRestitutionInput.text = grass.restitution.ToString(); 
            grassRestitutionInput.onEndEdit.AddListener(SetGrassRestitution); 
        }
        if (rockFrictionInput != null) { 
            rockFrictionInput.text = rock.friction.ToString(); 
            rockFrictionInput.onEndEdit.AddListener(SetRockFriction); 
        }
        if (rockRestitutionInput != null) {
            rockRestitutionInput.text = rock.restitution.ToString(); 
            rockRestitutionInput.onEndEdit.AddListener(SetRockRestitution); 
        }
        if (sandFrictionInput != null) { 
            sandFrictionInput.text = sand.friction.ToString(); 
            sandFrictionInput.onEndEdit.AddListener(SetSandFriction); 
        }
        if (sandRestitutionInput != null) { 
            sandRestitutionInput.text = sand.restitution.ToString(); 
            sandRestitutionInput.onEndEdit.AddListener(SetSandRestitution); 
        }
    }

    public void ApplyTerrain(TerrainType terrain)
    {
        if (terrain.surfaces == null) return;
        foreach (Surface s in terrain.surfaces)
        {
            if (s == null) continue;
            s.friction = terrain.friction;
            s.restitution = terrain.restitution;
        }
    }

    public void SetInitialForce(string value)
    {
        if (float.TryParse(value, out float result))
            initialForce = result;
    }

    public void SetMass(string value)
    {
        if (ball != null && float.TryParse(value, out float result)) 
            ball.mass = result;
    }

    public void SetGrassFriction(string value)
    {
        if (float.TryParse(value, out float result)) { 
            grass.friction = result; 
            ApplyTerrain(grass); 
        }
    }

    public void SetGrassRestitution(string value)
    {
        if (float.TryParse(value, out float result)) { 
            grass.restitution = result; 
            ApplyTerrain(grass); 
        }
    }

    public void SetRockFriction(string value)
    {
        if (float.TryParse(value, out float result)) { 
            rock.friction = result; 
            ApplyTerrain(rock); 
        }
    }

    public void SetRockRestitution(string value)
    {
        if (float.TryParse(value, out float result)) { 
            rock.restitution = result; 
            ApplyTerrain(rock); 
        }
    }

    public void SetSandFriction(string value)
    {
        if (float.TryParse(value, out float result)) { 
            sand.friction = result; 
            ApplyTerrain(sand); 
        }
    }

    public void SetSandRestitution(string value)
    {
        if (float.TryParse(value, out float result)) { 
            sand.restitution = result; 
            ApplyTerrain(sand); 
        }
    }

    public void ResetParams()
    {
        initialForce = defaultInitialForce;
        grass = defaultGrass;
        rock = defaultRock;
        sand = defaultSand;
        if (ball != null) ball.mass = defaultMass;

        ApplyTerrain(grass);
        ApplyTerrain(rock);
        ApplyTerrain(sand);

        if (initialForceInput != null)
            initialForceInput.text = initialForce.ToString();
        if (massInput != null && ball != null)
            massInput.text = ball.mass.ToString();
        if (grassFrictionInput != null)
            grassFrictionInput.text = grass.friction.ToString();
        if (grassRestitutionInput != null)
            grassRestitutionInput.text = grass.restitution.ToString();
        if (rockFrictionInput != null)
            rockFrictionInput.text = rock.friction.ToString();
        if (rockRestitutionInput != null)
            rockRestitutionInput.text = rock.restitution.ToString();
        if (sandFrictionInput != null)
            sandFrictionInput.text = sand.friction.ToString();
        if (sandRestitutionInput != null)
            sandRestitutionInput.text = sand.restitution.ToString();
    }
}
