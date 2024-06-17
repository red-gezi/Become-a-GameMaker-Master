using UnityEngine;

public class CharaManager : MonoBehaviour
{
    public float speed = 5f; // �ƶ��ٶ�
    public float jumpForce = 10f; // ��Ծ����
    private Rigidbody2D rb;
    public bool isGrounded; // �Ƿ��ŵ�
    public GameObject cubePrefab;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // ˮƽ�ƶ�
        float moveX = Input.GetAxisRaw("Horizontal");
        if (isGrounded)
        {
            rb.velocity=  new Vector2(-moveX * speed, rb.velocity.y);

        }
        // ��Ծ
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
        // �ӽ�ɫ�Ľ��·�������
        Vector2 rayOrigin = transform.position + (Vector3.down * 0.5f); // �����ɫ����ײ�������ڽ��£�����ƫ�ư����λ��ȷ�����ߴӽ��·���
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 10, LayerMask.GetMask("Ground")); // ����"Ground"�ǵ���������
        Debug.DrawRay(rayOrigin, Vector2.down * 10);
        if (hit.collider != null)
        {
            // ���߻����˵��棬���ɷ���
            Vector3 blockPosition = hit.point + new Vector2(0, 0.5f); // ��������λ��Ϊ���߻��еĵ�
            Instantiate(cubePrefab, blockPosition, Quaternion.identity); // ���ɷ���
        }
    }
    // ʾ��������ɫ�Ƿ�վ�ڵ����ϵĺ�������Ҫ���������Ϸ�߼�ʵ��
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