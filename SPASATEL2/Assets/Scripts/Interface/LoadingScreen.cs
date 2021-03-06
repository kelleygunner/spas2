﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour 
{

    public Text txtAdvice;


    string[] tips = 
    {

		"Играть со спичками и зажигалками - ОПАСНО. Игры с огнем могут стать причиной пожара",
		
		
		"Оставлять без присмотра включенные электроприборы - утюги, обогреватели, телевизор, светильники – ОПАСНО",
		
		
		"Уходя из дома, спроси у взрослых: Все электроприборы выключены?",
		
		"Почувствовал запах газа, скажи об этом взрослым. Если взрослых нет дома, покинь квартиру и позвони по телефону 112. При запахе газа включать свет и зажигать спички – ЗАПРЕЩЕНО",
		
		
		"Зажигать фейерверки, свечи или бенгальские огни дома – ОПАСНО",
		
		
		"В доме появился огонь? Немедленно уйди из помещения! Закрой дверь, но не запирай на ключ. Позвони соседям, предупреди их о возгорании и попроси вызвать пожарных. Если взрослых нет рядом - звони по телефону 112",
		
		
		"Если при пожаре не можешь покинуть квартиру, то сразу позвони по телефону 112. Запомни! Знать точный адрес и номер квартиры - ЖИЗНЕННО ВАЖНО. После звонка зови помощь соседей",
		
		
		"Дым гораздо ОПАСНЕЕ огня. Если чувствуешь, что задыхаешься, сядь на корточки и передвигайся ползком к безопасному месту – у пола дыма меньше",
		
		
		"Обязательно закрой дверь комнаты, где начался пожар. Закрытая дверь задержит проникновение дыма",
		
		
		"При пожаре пользоваться лифтом - ЗАПРЕЩЕНО. Спускайся по лестнице",
		
		
		"Запомните самое главное правило - в любой опасной ситуации сохраняй спокойствие!",
		
		"Пользуйся электроприборами только под присмотром взрослых",
		
		"Играть с розетками - ЗАПРЕЩЕНО",
		
		"Не трогай провода и электроприборы мокрыми руками",
		
		
		"Телефон экстренной помощи – 112",
		
		
		"Заметил пожар - звони 112",

        "Герои будут такого же цвета, что и в раскраске.", 
        "Старайтесь использовать приложение в хорошо освещенном месте.", "Рассмотрите персонажей получше, приблизив устройство к странице.", 
        "Не суши белье над плитой. Оно может загореться.",
        "Спасатель Степан Орлов не имеет особенных супер-способностей, но, благодаря упорному труду и тренировкам , он сильнее и быстрее обычного человека.", 
        "Кот Архимед - гениальный кот-изобретатель. Это самый умный кот на планете. Читает по 10 книг в день. ", 
        "Машина-трансформер - изобретение Архимеда, самый быстрый автомобиль после Бугатти Вейрон. Умеет трансформироваться во все необходимые виды транспорта. ", 
        "Диспетчер Лена - красивая, аккуратная, ответственная, добрая сотрудница МЧС. Прекрасный стратег и тактик.",
        "Кот Архимед носит очки собственного изобретения, которые моментально показывают правильную дорогу к месту происшествия. Умеет создавать сферу-защиту от огня."
    };

	void Start () 
    {
        txtAdvice.text = tips[Random.Range(0, tips.Length)];
	}
	

	void Update () 
    {
	
	}
}
