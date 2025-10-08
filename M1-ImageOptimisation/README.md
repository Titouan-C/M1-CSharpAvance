# Optimisation des images

## Résultats obtenus

- 14 images à optimiser
- 3 résolutions d'images (1080p, 720p, 480p)
- Taille totale de toutes les images (du dossier img) : 26.5 Mo

| Numéro d'essai | Version du programme | Résultat obtenu |
|----------------|---------------------|-----------------|
| 1              | Sans optimisation   | Sequential optimization time: 14523 ms |
| 1              | Avec optimisation   | Asynchronous optimization time: 2760 ms |
| 2              | Sans optimisation   | Sequential optimization time: 12969 ms |
| 2              | Avec optimisation   | Asynchronous optimization time: 2720 ms |
| 3              | Sans optimisation   | Sequential optimization time: 12850 ms |
| 3              | Avec optimisation   | Asynchronous optimization time: 2484 ms |
| 4              | Sans optimisation   | Sequential optimization time: 13879 ms |
| 4              | Avec optimisation   | Asynchronous optimization time: 2515 ms |
| 5              | Sans optimisation   | Sequential optimization time: 12363 ms |
| 5              | Avec optimisation   | Asynchronous optimization time: 2452 ms |

## Conclusion

En moyenne, l'optimisatio asynchrone (parallèle + asynchrone) est environ 5x plus rapide que l'optimisation séquentielle (sur mon pc, intel i7 11th gen, 32Go RAM).

Sans optimisation, le temps moyen est d'environ 13317 ms. Avec optimisation, le temps moyen est d'environ 2586 ms.
