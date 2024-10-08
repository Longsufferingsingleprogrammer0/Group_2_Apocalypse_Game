using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class knifeAttackScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private float XCenterOffset;
    [SerializeField] private float YCenterOffset;
    [SerializeField] private float horizontalRotation;
    [SerializeField] private float verticalRotation;
    [SerializeField] private float attackUpVerticalOffset;
    [SerializeField] private float attackDownVerticalOffset;
    [SerializeField] private float attackLeftHorizontalOffset;
    [SerializeField] private float attackRightHorizontalOffset;
    [SerializeField] private Vector2 returnPoint;
    [SerializeField] private bool randomAttackDamage;
    [SerializeField] private float attackDamageMin;
    [SerializeField] private float attackDamageMax;


    public float generateDamageValue()
    {
        if (randomAttackDamage)
        {
            return UnityEngine.Random.Range(attackDamageMin, attackDamageMax);
        }
        return attackDamageMax;
    }

    public void setPosition(Vector2 position, Quaternion rotation)
    {
        transform.SetPositionAndRotation(position, rotation);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy")) 
        {
            collision.gameObject.GetComponent<Enemy_Script>().damage(generateDamageValue());
          
        }
    }    
    
        
    public void attack(Vector2 playerpos, int PlayerDirection)
    {

        Vector2 adjustedPlayerPos = new Vector2(playerpos.x - 0.5f, playerpos.y + 1.2f);
        float x=0;
        float y=0;
        float rotation = 0;
        
        switch (PlayerDirection)
        {
            case 0:
                x = adjustedPlayerPos.x + XCenterOffset;
                y = adjustedPlayerPos.y + YCenterOffset + attackUpVerticalOffset;
                rotation = verticalRotation;                
                break;

            case 1:
                x = adjustedPlayerPos.x + XCenterOffset + attackLeftHorizontalOffset;
                y = adjustedPlayerPos.y + YCenterOffset;
                rotation = horizontalRotation;
                break;

            case 2:
                x = adjustedPlayerPos.x + XCenterOffset;
                y = adjustedPlayerPos.y + YCenterOffset + attackDownVerticalOffset;
                rotation = verticalRotation;
                break;

            case 3:
                x = adjustedPlayerPos.x + XCenterOffset + attackRightHorizontalOffset;
                y = adjustedPlayerPos.y + YCenterOffset;
                rotation = horizontalRotation;
                break;

            default:
                Debug.Log("unexpected rotation value given to attack");
                break;
        }
        setPosition(new Vector2(x, y), new Quaternion(0, 0, rotation, 0));
        StartCoroutine(attackWorker());
    }

   private IEnumerator attackWorker()
   {

        yield return new WaitForFixedUpdate();
        setPosition(returnPoint, new Quaternion(0, 0, 0, 0));
   }

    void Start()
    {
        setPosition(returnPoint, new Quaternion(0, 0, 0, 0));
    }

    private void FixedUpdate()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}
