[System.Serializable]
public class PlayerSettings {
    public float movementSpeed = 3;

    public float afterAttackFreeze = 0.2f;
    
    public float[] slashDuration = new float[] {
        0.5f,
        1
    };

    public int currentSlash = 0;
    public bool canAttackWhileMoving = false;
}
