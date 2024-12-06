# Simulação de Lançamento Oblíquo em Unity C#

Veja uma [demonstração](https://otavio.fun/fisica)!

## 1. Integrantes do Grupo e Responsabilidades
Disciplina: 7600105 - Física Básica I (2024).\
Docente: Krissia de Zawadzki.

|Nomes | N° USP | Atribuição|
|:------:|:-------:|:-----------:|
|Gustavo Alvares Andó|15457345|Código|
|Lucas Rodrigues Baptista|15577631|Código/Tester|
|Otávio Biagioni Melo|15482604|Interface de Usuário|
|Maria Clara de Souza Capato|15475294|Relatório/Tester|

## 2. Motivações e Descrição do Projeto
Este projeto é uma simulação criada para ilustrar a trajetória de um Lançamento Oblíquo, com o objetivo de trazer uma representação visual e interativa para os usuários. Houve um foco maior para a interface, de forma que ela seja acessível e intuitiva para o uso. Desenvolvido na Unity, a linguagem utilizada foi o C#, e os ícones e fontes são da LTWave.\
Quando pensamos em Lançamento Oblíquo estamos falando de uma trajetória com movimento em duas dimensões, comumente representadas pelos eixos x e y. Em aulas de física, a representação clássica do movimento é um tiro de canhão, ou um arremesso de um objeto de uma certa altura. Por isso, a representação escolhida buscou ser semelhante às mais comumente exploradas em aulas, trazendo uma visualização mais didática e lúdica e criando uma relação entre o que o usuário possa ter visto anteriormente em seus estudos.\
O projeto foi inicialmente influenciado por uma simulação do [Phet Colorado](https://phet.colorado.edu/pt_BR/simulations/projectile-motion), a qual traz o movimento de um projétil disparado por um canhão de acordo com as configurações escolhidas pelo usuário. Nossa simulação se espelhou nessa forma de interação, tentando fazer com que o usuário sinta-se livre para brincar com as condições iniciais e assistir como cada fator influencia na trajetória do corpo teste. \
Existem duas opções para a simulação, que podem ser comparadas ao mesmo tempo: a utilizando a resolução das EDO's que serão apresentadas a seguir (em azul) e a utilizando a integração de Verlet (em vermelho).
<!-- fotinho do projeto -->

## 3. Conceitos Físicos e Modelo Matemático
### Força Gravitacional
A força gravitacional $\vec{F}\_g$ pode ser expressa por:

$$\begin{equation}
\vec{F}\_g = -mg \hat{j},
\end{equation}$$

em que $m$ é a massa do corpo afetado, $g$ a gravidade e $\hat{k}$ o versor que indica a direção do movimento, neste caso, para cima. Vale notar que esta é uma força constante, que não depende da trajetória, apenas da gravidade local.

### Força Viscosa
A força viscosa $\vec{F}\_v$ é expressa por:

$$\begin{equation}
\vec{F}\_v = -b\vec{v},
\end{equation}$$

em que $b>0$ e $\vec{v}$ a velocidade do corpo afetado. Essa força tem relação direta com velocidade do corpo, variando de forma proporcial à ela no tempo, mas com sentido contrário.

### Movimento do Corpo e EDO's
Nosso projétil sofre a ação de ambas as forças descritas acima, tendo sua posição nos eixos $x$ (horizontal, orientado positivamente para a direita) e $y$ (vertical, orientado positivamente para cima) representada por:

$$\begin{equation}
\vec{r}(t) = x(t) \hat{i} + y(t) \hat{j}.
\end{equation}$$

Sua velocidade, derivada da posição, se dá por:

$$\begin{equation}
\vec{v}(t) = \dot{x}(t) \hat{i} + \dot{y}(t) \hat{j},
\end{equation}$$

e sua aceleração, derivada segunda da posição e primeira da velocidade é:

$$\begin{equation}
\vec{a}(t) = \ddot{x}(t) \hat{i} + \ddot{y}(t) \hat{j}.
\end{equation}$$

Como condições iniciais, temos:

$$\begin{align}
x(0) &= y(0) = 0,\\
\dot{x}(0) &= v_0 cos(\theta),\\
\dot{y}(0) &= v_0 sin(\theta).
\end{align}$$

Aplicando a 2ª Lei de Newton, chegamos às seguintes equações

$$\begin{align}
-mg\hat{j} -b\vec{v}(t) &= m(\ddot{x}(t)\hat{i} + \ddot{y}(t)\hat{j}),\\
\ddot{x}(t) + b{y}(t) &= 0,\\
\ddot{y}(t) + b{y}(t) + mg &= 0,
\end{align}$$

e às seguintes EDO's,

$$\begin{align}
\ddot{x}(t) &= \frac{-b}{m}\dot{x}(t),\\
\ddot{y}(t) &= \frac{-b}{m}\dot{y}(t),
\end{align}$$

as quais têm suas soluções dadas por:

$$\begin{align}
x(t) &= \tau v_0 \cos\theta (1- e^{\frac{-t}{\tau}}),\\
y(t) &= (v_0 \sin\theta + g\tau^{2})(1- e^{\frac{-t}{\tau}}) -g\tau t,
\end{align}$$

sendo $\tau$ definida como $\frac{m}{b}$.

### Integração de Verlet
O método de Verlet é um algoritmo utilizado para o cálculo das posições de um corpo, muito usado para simulações, como em nosso caso.\
Seu algoritmo busca reduzir o nível de erros nos cálculos utilizando as posições e aceleração anteriores para o cálculo da próxima posição. Assim, a cada pequena variação no tempo, a posição é recalculada e as variáveis são atualizadas para o próximo cálculo. Uma de suas vantagens é a conservação aproximada da energia do sistema, e é considerado um método mais estável que o de Euler. Suas equações são dadas por:

$$\begin{align}
\vec{S}\_{n+1} &= \vec{S} + \vec{v}\_n \cdot \Delta t + \dfrac{\vec{a}\_n}{2} \cdot \Delta t^2,\\
\vec{a}\_{n+1} &= \dfrac{\vec{F}\_n}{m}, \\
\vec{v}\_{n+1} &= \vec{v}\_n + \dfrac{\vec{a}\_n + \vec{a}\_{n+1}}{2} \Delta t,
\end{align}$$

em que $\vec{S}\_{n+1}$ é a próxima posição, $\vec{a}\_{n+1}$ a próxima aceleração e $\vec{v}\_{n+1}$ a próxima velocidade, todas após uma pequena variação de tempo.

### Colisões
Vale ressaltar que as colisões do corpo com o chão foram consideradas perfeitamente elásticas, e cada método lidou com estas de uma forma diferente. Na integração de Verlet, bastou verificar se existe uma colisão, e caso esta ocorra, a componente $y$ da velocidade é invertida. Já com as EDO's a colisão é tratada como um novo lançamento, com a inversão dessa mesma componente.
Além disso, o método de Newton foi utilizado para achar raízes das equações, as quais são os pontos de colisão do objeto com o solo.

$$\begin{equation}
t_{n+1} = t_n - \dfrac{y(t_n)}{\dot{y}(t_n)},
\end{equation}$$

em que $t_{n+1}$ é uma melhor aproximação do momento da colisão.

## 4. O Projeto
<!-- Como acessar o projeto -->
### Botões
Cada botão tem um ícone próprio, ilustrativo para sua função correspondente.
* Configurações: Abre uma aba para modificar as condições de lançamento;
* Lançar: Reinicia a simulação com as condições especificadas;
* Lupas: Aumentam, diminuem ou reiniciam o zoom da câmera (pode ser alterado pelo scroll do mouse);
* Setas: Movimentam a câmera horizontalmente (pode ser movido com as setinhas do teclado).

<!-- Print só dos botões 
<p align="center">
  <img src="imagens/Botoes.png" alt="Descrição da imagem">
  <br>
</p>
-->

### Configurações
Acessando as configurações, é possível modificar os parâmetros e condições iniciais livremente.
* Tipo de Simulação: escolha entre mostrar uma, nenhuma ou ambas as simulações (EDO's ou integração de Verlet);
* Gravidade: varia de 0 a 100, em m/s²;
* Velocidade inicial: varia de 0 a 100, em m/s;
* Ângulo inicial: varia de -90 a 90, em graus;
* Viscosidade: varia de 0 a 10, em kg/s;
* Altura Inicial: varia de 0 a 28, em metros;
* Massa do Projétil: varia de 0.1 a 100, em kg;
* Time Step da Simulação: varia de 5 a 120, em Hz;
* Colisões calculadas: varia de 0 a 100;
* Iterações do Método de Newton: varia de 0 a 10.

<!-- Print só da barra de configurações aberta 
<p align="center">
  <img src="imagens/Config.png" alt="Descrição da imagem">
  <br>
</p>
-->

## Referências
1. Phet Colorado, 2024. Disponível em: [Phet Colorado](https://phet.colorado.edu/pt_BR/simulations/projectile-motion). Acesso em 20 de novembro de 2024.
2. Bernardes, E. de S. (2024). Dinâmica-v4 (Notas de aula). 7600105 - Física Básica I. Universidade de São Paulo, São Carlos. Acesso em 28 de novembro de 2024.
3. VERLET integration. In: WIKIPÉDIA: a enciclopédia livre. Disponível em: https://en.wikipedia.org/wiki/Verlet_integration. Acesso em: 04 de dezembro de 2024. 
4. FREIRE, Wilson Hugo C.; MEDEIROS, Marciano L.; LEITE, Daniela; SILVA, Raffaela M. Lançamento oblíquo com resistência do ar: Uma análise qualitativa. SciELO Brasil, [s. l.], 30 set. 2015. Disponível em: https://doi.org/10.1590/S1806-11173812085. Acesso em: 05 de dezembro de 2024.
5. MÉTODO de Newton–Raphson. In: WIKIPÉDIA: a enciclopédia livre. Disponível em: https://pt.wikipedia.org/wiki/M%C3%A9todo_de_Newton%E2%80%93Raphson. Acesso em: 05 de dezembro de 2024. 
