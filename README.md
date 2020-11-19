# Unity_Project

## Tutorial Demo
[![Tutorial Demo](https://img.youtube.com/vi/1BVbqHLKkmM/maxresdefault.jpg)](https://youtu.be/1BVbqHLKkmM)



## terrain
[![Tutorial Demo](https://img.youtube.com/vi/foPcw3q9s14/sddefault.jpg)](https://youtu.be/foPcw3q9s14)

Perlin Noise를 사용해 Height map 생성 이후 Color 및 Texture 작업 후 Mesh 생성.

| 변수 | 설명 |
| Octave | Perline Noise를 생성할 갯수 |
| Lacunarity | Octave의 빈도 증가값 |
| Persistance | Octave의 진폭 감소값 |

1.	하나의 Perlin Noise를 사용하면 평평하기 때문에 여러 개의 Perline Noise 생성한다. 
예를 들어 octave1은 외곽선을 나타내면, octave2은 큰 암석, octave3은 작은 돌을 나타낸다.

2.	 각각의 Octave 들의 빈도와 진폭을 조절해 알맞은 Octave를 만들어 합친다.



## shader
[![Tutorial Demo](https://img.youtube.com/vi/3dsQ8QNqF7E/sddefault.jpg)](https://youtu.be/3dsQ8QNqF7E)
