// Variables para controlar las posiciones de los Pokémon
let pokemon1 = document.getElementById('pokemon1');
let pokemon2 = document.getElementById('pokemon2');
let battleArea = document.getElementById('battle-area');

// Posiciones iniciales
let pokemon1Position = { x: 100, y: 100 };
let pokemon2Position = { x: 600, y: 100 };

// Dimensiones del área de batalla
const battleAreaBounds = {
    width: battleArea.offsetWidth,
    height: battleArea.offsetHeight
};

// Dimensiones de los Pokémon
const pokemonSize = { width: 100, height: 100 };

// Movimiento con teclas
document.addEventListener('keydown', function (event) {
    const key = event.key;

    // Movimiento de Pokémon 1 (A, D, W, S)
    if (key === 'a') {
        pokemon1Position.x = Math.max(0, pokemon1Position.x - 10); // No salir por la izquierda
    } else if (key === 'd') {
        pokemon1Position.x = Math.min(battleAreaBounds.width - pokemonSize.width, pokemon1Position.x + 10); // No salir por la derecha
    } else if (key === 'w') {
        pokemon1Position.y = Math.max(0, pokemon1Position.y - 10); // No salir por arriba
    } else if (key === 's') {
        pokemon1Position.y = Math.min(battleAreaBounds.height - pokemonSize.height, pokemon1Position.y + 10); // No salir por abajo
    }

    // Movimiento de Pokémon 2 (Flechas)
    if (key === 'ArrowLeft') {
        pokemon2Position.x = Math.max(0, pokemon2Position.x - 10);
    } else if (key === 'ArrowRight') {
        pokemon2Position.x = Math.min(battleAreaBounds.width - pokemonSize.width, pokemon2Position.x + 10);
    } else if (key === 'ArrowUp') {
        pokemon2Position.y = Math.max(0, pokemon2Position.y - 10);
    } else if (key === 'ArrowDown') {
        pokemon2Position.y = Math.min(battleAreaBounds.height - pokemonSize.height, pokemon2Position.y + 10);
    }

    // Lanzar ataque (tecla Q para Pokémon 1, tecla Enter para Pokémon 2)
    if (key === 'q') {
        launchAttack(pokemon1Position, pokemon2Position, 'pokemon1');
    } else if (key === 'Enter') {
        launchAttack(pokemon2Position, pokemon1Position, 'pokemon2');
    }

    // Actualizar posiciones de los Pokémon
    pokemon1.style.left = pokemon1Position.x + 'px';
    pokemon1.style.top = pokemon1Position.y + 'px';
    pokemon2.style.left = pokemon2Position.x + 'px';
    pokemon2.style.top = pokemon2Position.y + 'px';
});

// Función para lanzar un ataque
function launchAttack(attackerPosition, defenderPosition, attackerId) {
    let attack = document.createElement('div');
    attack.style.position = 'absolute';
    attack.style.left = attackerPosition.x + 50 + 'px'; // Inicia desde el centro del Pokémon atacante
    attack.style.top = attackerPosition.y + 50 + 'px';
    attack.style.width = '10px';
    attack.style.height = '10px';
    attack.style.backgroundColor = 'yellow';
    attack.style.borderRadius = '50%';

    document.getElementById('battle-area').appendChild(attack);

    // Calcular la dirección del ataque hacia el oponente
    let deltaX = defenderPosition.x - attackerPosition.x;
    let deltaY = defenderPosition.y - attackerPosition.y;
    let distance = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
    let velocityX = (deltaX / distance) * 5; // Velocidad del proyectil en X
    let velocityY = (deltaY / distance) * 5; // Velocidad del proyectil en Y

    // Movimiento del ataque hacia el Pokémon defensor
    let attackInterval = setInterval(() => {
        let currentX = parseFloat(attack.style.left);
        let currentY = parseFloat(attack.style.top);

        // Verificar límites del área de batalla
        if (
            currentX < 0 || currentX > battleAreaBounds.width ||
            currentY < 0 || currentY > battleAreaBounds.height
        ) {
            clearInterval(attackInterval); // Detener el ataque
            attack.remove(); // Eliminar el proyectil
            return;
        }

        // Verificar si el ataque ha llegado al objetivo
        if (Math.abs(currentX - defenderPosition.x - 50) < 10 && Math.abs(currentY - defenderPosition.y - 50) < 10) {
            clearInterval(attackInterval); // Detener el ataque cuando llega al objetivo
            launchAttackEffect(defenderPosition, attackerId); // Llamar al efecto de colisión
            attack.remove(); // Eliminar el proyectil
        } else {
            attack.style.left = currentX + velocityX + 'px'; // Mover en X
            attack.style.top = currentY + velocityY + 'px'; // Mover en Y
        }
    }, 20);
}

// Función para manejar el efecto de colisión
function launchAttackEffect(defenderPosition, attackerId) {
    console.log(`${attackerId} ha golpeado a su oponente en (${defenderPosition.x}, ${defenderPosition.y})`);
}
