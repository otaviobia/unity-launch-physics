# Simulação de Lançamento Oblíquo em Unity C#

## 1. Integrantes do Grupo e Responsabilidades
Disciplina: 7600105 - Física Básica I (2024).\
Docente: Krissia de Zawadzki.

|Nomes | N° USP | Atribuição|
|------|-------|-----------|
|Gustavo Alvares Andó|15457345|Código|
|Lucas Rodrigues Baptista|15577631|Código|
|Otávio Biagioni de Melo|15482604|Interface de Usuário|
|Maria Clara de Souza Capato|15475294|Relatório|

## 2. Motivações e Descrição do Projeto
Este projeto é uma simulação criada para ilustrar a trajetória de um Lançamento Oblíquo, com o objetivo de trazer uma representação visual e interativa para os usuários. Houve um foco maior para a interface, de forma que ela seja acessível e intuitiva para o uso.\
Desenvolvido na Unity, a linguagem utilizado foi o C#.\ <!-- fonte dos ícones -->
Quando pensamos em Lançamento Oblíquo estamos falando de uma trajetória com movimento em duas dimensões, comumente representadas pelos eixos x e y. Em aulas de física, a representação clássica do movimento é um tiro de canhão, ou um arremesso de um objeto de uma certa altura. Por isso, a representação escolhida foi justamenta a de um disparo de canhão, buscando uma visualização mais didática e lúdica, e a criação de uma relação entre a representação mais comum dessa trajetória no ensino.\

## 3. Conceitos Físicos e Modelo Matemático
### Força Gravitacional
A força gravitacional $F_g$ pode ser expressa por:

$$\begin{equation}
F_g = -mg \hat{k},
\end{equation}$$

em que $m$ é a massa do corpo afetado, $g$ a gravidade e $\hat{k}$ o versor que indica a direção do movimento, neste caso, para cima. Vale notar que esta é uma força constante, que não depende da trajetória, apenas da gravidade local.
### Força Viscosa
A força viscosa $F_v$ é expressa por:

$$\begin{equation}
F_v = -bv,
\end{equation}$$

em que $b>0$ e $v$ a velocidade do corpo afetado. Essa força tem relação direta com velocidade do corpo, variando de forma proporcial à ela no tempo, mas com sentido contrário.

### Sistema de Coordenadas

### Movimento do Corpo
Nosso projétil sofre a ação de ambas as forças descritas acima, tendo sua posição representada por:

$$\begin{equation}
r(t) = x(t) \hat{i} + y(t) \hat{j} + z(t) \hat{k}.
\end{equation}$$

Sua velocidade, derivada da posição, se dá por:

$$\begin{equation}
v(t) = \dot{x}(t) \hat{i} + \dot{y}(t) \hat{j} + \dot{z}(t) \hat{k},
\end{equation}$$

e sua aceleração, derivada segunda da posição e primeira da velocidade é:

$$\begin{equation}
v(t) = \ddot{x}(t) \hat{i} + \ddot{y}(t) \hat{j} + \ddot{z}(t) \hat{k}.
\end{equation}$$

Como condições iniciais, temos:

$$\begin{equation}
x(0) = y(0) = z(0) = 0,
\end{equation}$$

e

$$\begin{align}
\dot{x}(0) &= 0 ,\\
\dot{y}(0) &= v_0 cos(\theta) ,\\
\dot{z}(0) &= v_0 sin(\theta) .
\end{align}$$

## 4. O Projeto
<!-- Como acessar o projeto -->
### Botões
Cada botão tem um ícone próprio, ilustrativo para sua função correspondente.
* Configurações: Abre uma aba para modificar as condições de lançamento;
* Lançar: Reinicia a simulação com as condições especificadas;
* Lupas: Aumentam, diminuem ou reiniciam o zoom da câmera;

### Configurações
Acessando as configurações, é possível modificar os parâmetros e condições iniciais livremente.
* Gravidade: varia de 0,01 a 100, em m/s².
* Massa do Projétil: varia de 0,01 a 100, em kg;
* Ângulo inicial: varia de 0 a 90, em graus;
* Velocidade inicial: varia de 0,01 a 100, em m/s;
* Viscosidade: varia de 0.01 a 10, em m²/s;
* Altura Inicial: varia de 0 a 28, em metros.

## Referências
1. Phet Colorado, 2024. Disponível em: [Phet Colorado](https://phet.colorado.edu/pt_BR/simulations/projectile-motion). Acesso em 20 de novembro de 2024.
2. Bernardes, E. de S. (2024). Dinâmica-v4 (Notas de aula). 7600105 - Física Básica I. Universidade de São Paulo, São Carlos. Acesso em 28 de novembro de 2024.
