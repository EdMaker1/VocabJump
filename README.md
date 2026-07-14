# VocabJump

**Plataformero 2D para el aprendizaje de vocabulario en inglés (A1-A2)**

VocabJump es un juego serio de plataformas en 2D desarrollado en Unity. El jugador controla a un personaje que avanza por niveles respondiendo preguntas de vocabulario en inglés de nivel A1-A2 (según el Marco Común Europeo de Referencia). Cada respuesta correcta materializa plataformas que abren el camino hacia la meta: aprender palabras nuevas **es** la mecánica central del juego, no un añadido.

> Proyecto final del curso de Desarrollo de Videojuegos — Escuela Profesional de Ingeniería de Software, Universidad La Salle (Arequipa, Perú).

## Capturas

| Menú principal | Gameplay | Panel de pregunta |
|---|---|---|
| ![Menú](docs/screenshots/menu.png) | ![Gameplay](docs/screenshots/gameplay.png) | ![Pregunta](docs/screenshots/question.png) |

*(Coloca tus capturas en `docs/screenshots/` con esos nombres, o ajusta las rutas.)*

## Características

- **Mecánica léxica integrada:** las plataformas se muestran translúcidas e intangibles; responder correctamente una pregunta de vocabulario las materializa con un fundido y habilita su colisión.
- **Banco de palabras extensible:** cada palabra es un ScriptableObject con término, traducción, categoría (colores, números, animales, objetos del hogar), dificultad (A1/A2) y 3 distractores. Agregar vocabulario no requiere tocar código.
- **Dificultad progresiva:** Nivel 1 con vocabulario A1 (2 preguntas), Nivel 2 con vocabulario A2 (3 preguntas), recorrido más largo y más obstáculos.
- **Sistema de vidas y penalizaciones:** 3 vidas por nivel; se pierden por respuestas incorrectas o contacto con obstáculos (con knockback e invulnerabilidad breve). Destello rojo de feedback y reintento con cuenta regresiva.
- **HUD completo:** corazones de vida, contador de palabras aprendidas y barra de progreso del nivel.
- **Flujo completo de juego:** splash con logo → menú principal (con instrucciones) → login local con persistencia del nombre (PlayerPrefs) → niveles → resúmenes por nivel y final con porcentaje de aciertos y palabras adquiridas.
- **Game feel:** salto con *coyote time* y *jump buffering*, cámara con seguimiento suave.

## Tecnologías

| Componente | Tecnología |
|---|---|
| Motor | Unity 2022.3.34f1 LTS (plantilla 2D Core) |
| Lenguaje | C# |
| Físicas | Unity Physics 2D (Rigidbody2D, Collider2D) |
| Entrada | Unity Input System (paquete nuevo, clase C# generada) |
| UI | Unity Canvas + TextMeshPro |
| Datos | ScriptableObjects (banco de palabras), PlayerPrefs (perfil local) |
| Control de versiones | Git + GitHub |
| Arte y audio | Kenney (assets gratuitos) |

## Arquitectura

**Flujo de escenas:**

```
SplashScreen → MainMenu → Login → Level1 → LevelSummary → Level2 → FinalSummary
```

**Patrones de diseño aplicados:**

- **Singleton** — `GameManager` (estado global: vidas, estadísticas) y `QuestionManager` (presentación y validación de preguntas).
- **Observer** — `GameEvents` centraliza los eventos del juego (`OnCorrectAnswer`, `OnWrongAnswer`, `OnLivesChanged`, etc.). El HUD, el flash de penalización y el gestor de partida se suscriben sin acoplarse entre sí: el sistema de vidas se añadió en una fase posterior sin modificar el sistema de preguntas.
- **Máquina de estados** — el flujo global se modela con las escenas como estados; dentro del juego, la alternancia exploración/pregunta se controla con `Time.timeScale`.

**Scripts principales:**

```
Assets/_Project/Scripts/
├── Player/
│   ├── PlayerController.cs    # movimiento, salto (OverlapCircle, coyote time, jump buffer)
│   └── CameraFollow.cs        # seguimiento suave de cámara
├── Managers/
│   ├── GameManager.cs         # singleton: vidas, estadísticas, game over
│   ├── GameEvents.cs          # canal de eventos (Observer)
│   ├── SessionData.cs         # datos entre escenas (nombre, resultados)
│   ├── SceneFlowController.cs # navegación entre escenas
│   ├── LevelExit.cs           # meta del nivel, registra estadísticas
│   └── Obstacle.cs            # daño por contacto, knockback, invulnerabilidad
├── Questions/
│   ├── WordData.cs            # ScriptableObject del banco de palabras
│   ├── QuestionManager.cs     # panel de preguntas, barajado, reintento
│   ├── QuestionZone.cs        # zonas trigger que lanzan preguntas
│   └── ActivatablePlatform.cs # plataformas fantasma → sólidas
└── UI/
    ├── HUDController.cs       # corazones, contador, barra de progreso
    ├── ScreenFlash.cs         # destello rojo de penalización
    ├── SplashController.cs    # fade del logo
    ├── LoginController.cs     # ingreso de nombre + PlayerPrefs
    └── SummaryController.cs   # resúmenes parcial y final
```

## Cómo ejecutar el proyecto

### Opción A — Abrir en Unity (código fuente)

1. Clonar el repositorio:
   ```bash
   git clone https://github.com/EdMaker1/VocabJump.git
   ```
2. Abrir **Unity Hub** → **Add** → seleccionar la carpeta clonada.
3. Abrir con **Unity 2022.3.34f1** (u otra versión 2022.3 LTS). Unity regenerará la carpeta `Library` y descargará los paquetes automáticamente en el primer arranque.
4. Abrir la escena `Assets/_Project/Scenes/SplashScreen` y presionar **Play**.

> El repositorio solo versiona `Assets`, `Packages` y `ProjectSettings` (`.gitignore` estándar de Unity); las carpetas `Library`, `Temp` y similares se regeneran localmente.

### Opción B — Ejecutable (build)

1. Descargar el `.zip` de la sección **Releases** (si está publicado).
2. Descomprimir y ejecutar `VocabJump.exe` (Windows 10+ de 64 bits).

## Controles

| Acción | Tecla |
|---|---|
| Moverse | `A` / `D` o flechas ← → |
| Saltar | `Espacio` (solo apoyado en el suelo) |
| Responder / navegar menús | Clic izquierdo |

## Cómo jugar

1. Escribe tu nombre en la pantalla de inicio (queda guardado para próximas sesiones).
2. Avanza por el nivel y entra en las **zonas de pregunta**: el juego se pausa y muestra una pregunta de vocabulario con 4 alternativas.
3. **Acierta** para materializar las plataformas translúcidas y seguir subiendo. **Falla** y pierdes una vida, con unos segundos de espera antes de reintentar.
4. Evita los obstáculos: también quitan vida.
5. Llega al marcador verde para completar el nivel y ver tu resumen: porcentaje de aciertos y palabras nuevas aprendidas.

## Requisitos funcionales cubiertos

1. Controlador de movimiento horizontal con velocidad configurable desde el Inspector.
2. Salto con detección de suelo mediante `Physics2D.OverlapCircle`.
3. Preguntas de vocabulario A1-A2 en zonas de activación del nivel.
4. Habilitación dinámica de plataformas al responder correctamente.
5. Penalización visual y de vida con reintento temporizado.
6. Banco de palabras organizado por categorías temáticas (colores, números, animales, objetos del hogar).
7. Sistema de vidas con reducción por obstáculos y respuestas incorrectas.
8. HUD con palabras aprendidas, vidas restantes y progreso del nivel.
9. Dos niveles con incremento progresivo de dificultad (A1 → A2).
10. Pantalla de resumen por nivel con porcentaje de aciertos y palabras adquiridas.

## Documentación

- **Manual de Usuario** — instalación, interfaz, controles y reglas de juego.
- **Memoria Técnica Descriptiva** — justificación pedagógica, arquitectura, patrones de diseño, implementación de requisitos y pruebas.

*(Ambos documentos se encuentran en la carpeta `docs/` del repositorio o se entregan junto al proyecto.)*

## Créditos

- **Desarrollo:** Eduardo Ccama — [@EdMaker1](https://github.com/EdMaker1)
- **Arte y sonido:** [Kenney](https://kenney.nl) (assets de dominio público, licencia CC0)
- **Motor:** [Unity](https://unity.com) 2022.3 LTS

## Licencia

Proyecto académico desarrollado con fines educativos para el curso de Desarrollo de Videojuegos. Los assets de Kenney se utilizan bajo licencia CC0.
