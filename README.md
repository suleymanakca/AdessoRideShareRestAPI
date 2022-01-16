# AdessoRideShareRestAPI

Kullanıcıların A şehirinden B şehirine şahsi arabaları ile seyehat ederken yolcu bulabileceği API'nin RESTful olacak şekilde yazılması.

## Kullanıcı seyahat bilgilerini ekleyebilir. Bilgiler; Nereden, Nereye, Tarih ve Açıklama, Koltuk Sayısı.

**Endpoint: /api/Trip**

**Metot: POST**

**RequestBody**

            id	integer($int32)
            nereden	string
            nullable: true
            nereye	string
            nullable: true
            tarih	string($date-time)
            aciklama	string
            nullable: true
            koltukSayisi	integer($int32)
            yayinda	boolean
            userID	integer($int32)

## Kullanıcı tanımladığı seyahat planını yayına alabilir ve yayından kaldırabilir

**Endpoint: /api/Trip/publish**

**Metot: PUT**

**RequestBody**

            tripId	integer($int32)
            yayinda	boolean


## Kullanıcılar sistemdeki yayında olan seyahat planlarını Nereden ve Nereye bilgileri ile aratabilir

**Endpoint: /api/Trip**

**Metot: GET**

**QueryParams**
          nereden string (query)	
          nereye  string (query)

## Kullanıcılar yayında olan seyehat planlarına "Koltuk Sayısı" dolana kadar katılım isteği gönderebilir

**Endpoint: /api/Trip/join**

**Metot: POST**

**RequestBody**

          id	integer($int32)
          userID	integer($int32)
          tripID	integer($int32)

