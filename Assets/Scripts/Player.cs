using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Camera camera;
//    float ySpeed;
//    float xSpeed;

	private static float TWO_PI = Mathf.PI * 2f;
//	private static float BEFORE_PI = Mathf.PI * 0.85;


	public static float minXSpeed = 5f;
//    public float maxXSpeed;
//    public float maxYSpeed;

    [SerializeField] TrailRenderer trailRend;
    [SerializeField] Color fixedColor;
    [SerializeField] Color freeColor;
	[SerializeField] float amplitude = 1f;
	[SerializeField] float periodDuration = 5f; // duracion de la recorrida de un periodo entero
										        // (desde que pasa por y = 0 hacia arriba, llega al tope y vuelve a bajar a y = 0)
	[SerializeField] float xSpeed = 2f;
	[SerializeField] float turnRadius = 0.05f; // Tiene que estar entre 0f y 0.25f (idealmente no en el borde)
	[SerializeField] float lerpDuration = 0.2f; // Tiempo que demora en pasar de la amplitud actual a la nueva
	float targetAmplitude;
	float lerpSpeed;
	bool lerpIncreasesAmplitude;
	bool lerping;
	float lerpTimer;

	//    float lastY;
	//    int movingSign;
	//    public float amp;
//    [SerializeField] float accelSpeed;

//    public float timeOffset;
//    float breakCooldown;

    Vector3 playerPosition = Vector2.zero;
	Vector3 lastPlayerMovement = Vector2.zero;
	Vector3 cameraPosition = Vector2.zero;
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
		float result = - (amp * 2.5f) + 10;
		return result;
	}

	private float getPeriodDuration(float amp) {
//		if (lerping) {
//			
//		} else {
			return amp * 2f;
//		}
	}


	void Awake() {
		transform.position = new Vector3 (-4f, 0f, 0f);
	}

    void Start() {
        isPressing = false;
        isMovingFree = false;
//        movingSign = 1;
//        amp = 0.1f;
		playerPosition = transform.position;
		cameraPosition = camera.transform.position;
		cameraDeltaX = cameraPosition.x - playerPosition.x;
//		ySpeed = maxXSpeed;

		UpdateTrailColor();
    }

	void Update() {
//		periodTimer = periodTimer % TWO_PI;

		isPressing = Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0);

		if (isMovingFree) {
			if (isPressing) {
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
				if (playerPosition.y > 0f) {
					// El jugador esta arriba de la horizontal
					if (lastPlayerMovement.y > 0f) {
						// Estoy yendo hacia arriba (primer mitad de la curva). Quiero ajustar las variables para estar en el punto 'a'
						curvePercentage = 0.25f - turnRadius; // a

					} else {
						// Estoy yendo hacia abajo (segunda mitad de la curva). Quiero ajustar las variables para estar en el punto 'b'
						curvePercentage = 0.25f + turnRadius; // b
					}

				} else if (playerPosition.y < 0f) {
					// El jugador esta abajo de la horizontal
					if (lastPlayerMovement.y < 0f) {
						// Estoy yendo hacia abajo (primer mitad de la curva) Quiero ajustar las variables para estar en el punto 'c'
						curvePercentage = 0.75f - turnRadius; // c

					} else {
						// Estoy yendo hacia arriba (segunda mitad de la curva) Quiero ajustar las variables para estar en el punto 'd'
						curvePercentage = 0.75f + turnRadius; // d
					}
				} else {
					
				}

				float periodPercentage = curvePercentage * TWO_PI;
				float currentHeightPercentage = Mathf.Sin (periodPercentage);

				lerping = true;
				lerpTimer = 0f;

				setAmplitude (Mathf.Abs (playerPosition.y / currentHeightPercentage));
//				targetAmplitude = Mathf.Abs(playerPosition.y / currentHeightPercentage);
//				lerpIncreasesAmplitude = targetAmplitude > amplitude;
				lerpSpeed = (targetAmplitude - amplitude) / lerpDuration;

				Debug.Log ("curvePercentage = " + curvePercentage + ", periodPercentage = " + periodPercentage
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
		if (!isMovingFree) {
			if (isPressing) {
				// comienzo a moverme libre
				isMovingFree = true;
				playerPosition += lastPlayerMovement;
			} else {
				// oscilo
				playerPosition.x += xSpeed * Time.deltaTime;
				
				float frequencyMultiplier = TWO_PI / periodDuration;
				float targetY = Mathf.Sin (periodTimer * frequencyMultiplier) * amplitude;
//				if (lerping) {
//					lerpTimer += Time.deltaTime;
//				} else {
					playerPosition.y = targetY;
//				}
			}
		}

		lastPlayerMovement = playerPosition - transform.position;

		transform.position = playerPosition;

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
		amplitude = newAmp;

		// float frequencyMultiplier = TWO_PI / periodDuration;
		// playerPosition.y = Mathf.Sin(periodTimer * frequencyMultiplier) * amplitude;

		xSpeed = getXSpeed (amplitude);
		periodDuration = getPeriodDuration (amplitude);
		Debug.Log ("Setting new amplitude " + amplitude + ", xSpeed = " + xSpeed + ", periodDuration = " + periodDuration);
	}
}
