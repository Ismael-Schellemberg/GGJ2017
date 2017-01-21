using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public Text score;
    int points;
    public LevelManager manager;
    [SerializeField] Animator anim;
    bool playing;
    public CameraEffects cameraEffects;
    public Camera camera;

    public static float TWO_PI = Mathf.PI * 2f;
    public static float minXSpeed = 5f;

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] TrailRenderer trailRend;
    [SerializeField] Color fixedColor;
    [SerializeField] Color freeColor;
    [SerializeField] float amplitude = 1f;
    // La amplitud de la oscilacion
    [SerializeField] float periodDuration = 5f;
    // duracion de la recorrida de un periodo entero (depende de amplitude)
    [SerializeField] float xSpeed = 2f;
    // Velocidad horizontal (depende de amplitude)
    [SerializeField] float turnRadius = 0.05f;
    // Tiene que estar entre 0f y 0.25f (idealmente no en el borde)
    //    [SerializeField] float lerpDuration = 0.2f;
    [SerializeField] float rotationMax = 35f;
    // cuanto varia el angulo del jugador
    // Tiempo que demora en pasar de la amplitud actual a la nueva
    float targetAmplitude;
    //    float lerpSpeed;
    //    bool lerpIncreasesAmplitude;
    //    bool lerping;
    //    float lerpTimer;

    Vector3 playerPosition = Vector3.zero;
    Vector3 spriteRotation = Vector3.zero;
    Vector3 lastPlayerMovement = Vector3.zero;
    Vector3 cameraPosition = Vector3.zero;
    float cameraDeltaX;

    bool isMovingFree;
    bool isPressing;

    float periodTimer = 0f;

    // amplitude 0   - xSpeed 10
    // amplitude 0.2 - xSpeed 9.5
    // amplitude 2   - xSpeed 5

    // amplitude 0.2 - periodDuration 0.4
    // amplitude 2   - periodDuration 4

    private float getXSpeed(float amp) {
        return 5f;
//        float result = -(amp * 2.5f) + 10;
//        return result;
    }

    private float getPeriodDuration(float amp) {
        return amp * 2f;
    }

    private float getRotationMax(float amp) {
        if(amp > 1f)
            return 35f;
        else {
            return 35f * amp;
        }
    }

    void Start() {
        if(spriteRenderer == null)
            spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
		
        isPressing = false;
        isMovingFree = false;
        playerPosition = transform.position;
        cameraPosition = camera.transform.position;
        cameraDeltaX = cameraPosition.x - playerPosition.x;
        setAmplitude(amplitude); // Solo para actualizar las cosas que dependen de la amp

        UpdateTrailColor();
        Reset();
    }

    void Update() {
        if(!playing)
            return;

//		periodTimer = periodTimer % TWO_PI;

        isPressing = Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0);

        if(isMovingFree) {
            if(isPressing) {
                playerPosition += lastPlayerMovement;
            } else {
                // Tengo que terminar de moverme libremente y volver a oscilar.

                // EXPLICACION ALGORITMO
                // La idea es marcar en que posicion de la curva es que estoy cuando termina el free movement
                // El turn radius es que tanto antes/despues de estar en el punto maximo de la curva es que esta a/b
                // El punto alto de la curva esta en curvePercentage = 0.25
                // El centro ('t') esta en curvePercentage = 0.5
                // El punto bajo de la curva esta en curvePercentage = 0.75
                // Un turn radius de 0.05 significa que:
                // a = 0.20
                // b = 0.30
                // c = 0.70
                // d = 0.80
                // | a_b
                // | / \
                // |/__ \t____________________
                // |     \   /
                // |      \_/
                // |      c d


                float curvePercentage = 0.5f;
                if(playerPosition.y > 0f) {
                    // El jugador esta arriba de la horizontal
                    if(lastPlayerMovement.y > 0f) {
                        // Estoy yendo hacia arriba (primer mitad de la curva). Quiero ajustar las variables para estar en el punto 'a'
                        curvePercentage = 0.25f - turnRadius; // a

                    } else {
                        // Estoy yendo hacia abajo (segunda mitad de la curva). Quiero ajustar las variables para estar en el punto 'b'
                        curvePercentage = 0.25f + turnRadius; // b
                    }

                } else if(playerPosition.y < 0f) {
                    // El jugador esta abajo de la horizontal
                    if(lastPlayerMovement.y < 0f) {
                        // Estoy yendo hacia abajo (primer mitad de la curva) Quiero ajustar las variables para estar en el punto 'c'
                        curvePercentage = 0.75f - turnRadius; // c

                    } else {
                        // Estoy yendo hacia arriba (segunda mitad de la curva) Quiero ajustar las variables para estar en el punto 'd'
                        curvePercentage = 0.75f + turnRadius; // d
                    }
                } else {
					
                }

                float periodPercentage = curvePercentage * TWO_PI;
                float currentHeightPercentage = Mathf.Sin(periodPercentage);

//                lerping = true;
//                lerpTimer = 0f;

                setAmplitude(Mathf.Abs(playerPosition.y / currentHeightPercentage));
//				targetAmplitude = Mathf.Abs(playerPosition.y / currentHeightPercentage);
//				lerpIncreasesAmplitude = targetAmplitude > amplitude;
//                lerpSpeed = (targetAmplitude - amplitude) / lerpDuration;

                Debug.Log("curvePercentage = " + curvePercentage + ", periodPercentage = " + periodPercentage
                + ", currentHeightPercentage = " + currentHeightPercentage + ", targetAmplitude = " + targetAmplitude);

					
                periodTimer = periodDuration * curvePercentage; // Ajusto el tiempo en el que estoy para que no se genere el salto
                Debug.Log("new periodTimer = " + periodTimer);
                isMovingFree = false;
            }
        }

        periodTimer += Time.deltaTime;

//		if (lerping) {
//			float newAmplitude = amplitude + (lerpSpeed * Time.deltaTime);
//			if (lerpIncreasesAmplitude) {
//				if (newAmplitude >= targetAmplitude) {
//					newAmplitude = targetAmplitude;
//					lerping = false;
//				}
//			} else {
//				if (newAmplitude <= targetAmplitude) {
//					newAmplitude = targetAmplitude;
//					lerping = false;
//				}
//			}
//			setAmplitude (newAmplitude);
//		}

        // Esto no puede estar en un else, porque el instante en que dejo de moverme libre tengo que controlar el movimiento aca y no arriba
        if(!isMovingFree) {
            if(isPressing) {
                // comienzo a moverme libre
                isMovingFree = true;
                playerPosition += lastPlayerMovement;
            } else {
                // oscilo
                playerPosition.x += xSpeed * Time.deltaTime;
				
                float frequencyMultiplier = TWO_PI / periodDuration;
                float periodTime = periodTimer * frequencyMultiplier;

                float periodSin = Mathf.Sin(periodTime);
                float periodCos = Mathf.Cos(periodTime);

                playerPosition.y = periodSin * amplitude;

                spriteRotation.z = -35 + (periodCos * rotationMax);

                // periodTime = 0      ... cos = 1  ... rotation = -35 + rotationMax
                // periodTime = PI/2   ... cos = 0  ... rotation = -35
                // periodTime = PI     ... cos = -1 ... rotation = -35 - rotationMax
                // periodTime = PI*3/2 ... cos = 0  ... rotation = -35
            }
        }

        lastPlayerMovement = playerPosition - transform.position;

        transform.position = playerPosition;
        spriteRenderer.transform.eulerAngles = spriteRotation;

        cameraPosition.x = playerPosition.x + cameraDeltaX;
        camera.transform.position = cameraPosition;
    }

    void UpdateTrailColor() {
        if(isMovingFree)
            trailRend.materials[0].SetColor("_TintColor", freeColor);
        else
            trailRend.materials[0].SetColor("_TintColor", fixedColor);
    }


    void setAmplitude(float newAmp) {
//		if (newAmp <= 
        amplitude = newAmp;

        // float frequencyMultiplier = TWO_PI / periodDuration;
        // playerPosition.y = Mathf.Sin(periodTimer * frequencyMultiplier) * amplitude;

        xSpeed = getXSpeed(amplitude);
        periodDuration = getPeriodDuration(amplitude);
        rotationMax = getRotationMax(amplitude);
        Debug.Log("Setting new amplitude " + amplitude + ", xSpeed = " + xSpeed + ", periodDuration = " + periodDuration);
    }

    void OnCollisionEnter2D(Collision2D coll) {
        if(!playing)
            return;

        if(coll.gameObject.tag == "Wall" || coll.gameObject.tag == "Obstacle") {
            cameraEffects.ShakeCamera(5f, 0.01f);
            playing = false;
            trailRend.enabled = false;
            anim.SetTrigger("die");  
        }
    }

    public void OnBornAnimationEnded() {
        playing = true;
        trailRend.enabled = true;
        trailRend.Clear();
    }

    public void OnDieAnimationEnded() {
        Reset();
        anim.SetTrigger("start");
    }

    void Reset() {
        UpdateScore();
        points = 0;
        playerPosition = new Vector3(-4f, 0f, 0f);
        transform.position = playerPosition;
        cameraPosition.x = 0;
        camera.transform.position = cameraPosition;
        isPressing = false;
        isMovingFree = false;
        UpdateTrailColor();

        manager.Reset();
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(!playing)
            return;

        if(col.tag == "Coin") {
            points++;
            UpdateScore();
            col.GetComponent<Coin>().Hit();
        }
    }

    void UpdateScore() {
        score.text = points.ToString();
    }
}
