Hello, thanks for taking the time to evaluate my case project. I would like to point out that I had to deal with
health problems within my family and could only work on the project from November 18 to November 20, so this project contains 3 days of work.
Below, I explain what I managed to implement during this time frame and what I did not have enough time to do.

What I did:
-An MVP(Model-View-Presenter) architectural pattern was used throughout the project. MatchGameManager.cs initializes everything and the systems start to work on their own after that.
-I used Addressables to load and unload sprites when levels are loaded.
-I implemented an event bus system that lets classes to subscribe to events, unsubscribe from events and publish events so that
different classes can interact with what is going on throughout the project.
-I added an infinite level loop
-You can zoom in, zoom out and drag the map, giving you access to different parts of it.
-I used Assembly Definitions to separate systems, increasing build and compilation time.
-Tweens were used for some animations.
-There is a goal system which shows the requirements of the level.
-There is a collection system which shows the current matchlings in your collection slots.
-The sprites were generated using ChatGPT. They were only optimized from their sprite settings. Sorry, I'm not an artist. :)

What I didn't do because of time related problems:
-A main menu
-A more generic load-save system where the data of slots, goals and time would also be saved
-Haptics and audio
-A loading screen between levels which would solve the addressable loading delay
-Add better UI

What I couldn't complete 100% because of time related problems:
-Couldn't 100% finalize the match animation when 3 same matchlings come together and when the other matchlings slide left.

GitHub Link: github.com/EkinDS/matchingham-test