using UnityEngine;

public class CharaManager : MonoBehaviour
{
    public float speed = 5f; // 移动速度
    public float jumpForce = 10f; // 跳跃力量
    private Rigidbody2D rb;
    public bool isGrounded; // 是否着地
    public GameObject cubePrefab;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 水平移动
        float moveX = Input.GetAxisRaw("Horizontal");
        if (isGrounded)
        {
            rb.velocity=  new Vector2(-moveX * speed, rb.velocity.y);

        }
        // 跳跃
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    GenerateBlockUnderFeet();
        //}
    }
    void GenerateBlockUnderFeet()
    {
        // 从角色的脚下发射射线
        Vector2 rayOrigin = transform.position + (Vector3.down * 0.5f); // 假设角色的碰撞体中心在脚下，向下偏移半个单位以确保射线从脚下发出
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 10, LayerMask.GetMask("Ground")); // 假设"Ground"是地面层的名称
        Debug.DrawRay(rayOrigin, Vector2.down * 10);
        if (hit.collider != null)
        {
            // 射线击中了地面，生成方块
            Vector3 blockPosition = hit.point + new Vector2(0, 0.5f); // 方块生成位置为射线击中的点
            Instantiate(cubePrefab, blockPosition, Quaternion.identity); // 生成方块
        }
    }
    // 示例：检测角色是否站在地面上的函数，需要根据你的游戏逻辑实现
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }
}