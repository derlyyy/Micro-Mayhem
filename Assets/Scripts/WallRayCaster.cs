using UnityEngine;

public class WallRayCaster : MonoBehaviour
{
    public float slideSpeed = 2f; // Скорость скольжения
    public float wallSlideTime = 0.5f; // Время, в течение которого персонаж может скользить по стене
    public LayerMask wallMask; // Слой стены
    public CharacterData characterData; // Ссылка на данные персонажа

    private float timeSinceWallTouch; // Время, прошедшее с момента касания стены

    private void Update()
    {
        if (characterData.isWallGrab)
        {
            // Увеличиваем время с момента касания стены
            timeSinceWallTouch += Time.deltaTime;

            // Устанавливаем скорость персонажа, чтобы он скользил вниз
            Vector2 slideVelocity = new Vector2(characterData.playerVel.rb.velocity.x, -slideSpeed);
            characterData.playerVel.rb.velocity = slideVelocity;
        }
        else
        {
            // Сбрасываем время, если не касаемся стены
            timeSinceWallTouch = 0f;
        }
    }

    private void FixedUpdate()
    {
        // Проверка на скольжение по стене
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, 0.7f, wallMask);
        if (hit.collider != null && timeSinceWallTouch < wallSlideTime)
        {
            characterData.isWallGrab = true; // Устанавливаем флаг зацепления за стену
            characterData.wallNormal = hit.normal; // Получаем нормаль стены
        }
        else
        {
            characterData.isWallGrab = false; // Убираем флаг зацепления за стену
        }
    }
}
