let connection = null;
setupSignalR();
let isLoading = true;
loading();
let clubs = [];
let selectedClubIndex = 0;
let selectedClub = null;
getData().then(x => updateForm()).then(x => loading());

function setupSignalR() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:44374/hub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on("ClubUpdated", (club, message) => {
        selectedClub = club;
    });

    connection.on("PlayerCreated", (player, message) => {
        selectedClub.players.push(player);
    });

    connection.on("ClubDeleted", (user, message) => {
        getData();
    });

    connection.on("PlayerDeleted", (player, message) => {
        selectedClub.players = selectedClub.players.filter(x => x.playerId != player.playerId);
    });

    connection.onclose(async () => {
        await start();
    });
    start();
}

async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

function selectClub() {
    const select = document.getElementById("clubs");
    selectedClub = clubs[select.selectedIndex];
    selectedClubIndex = select.selectedIndex;
    players();
    club();
    manager();
    updateForm();
}

function updateForm() {
    document.getElementById('clubname').value = selectedClub.clubName;
    document.getElementById('colour').value = selectedClub.clubColour;
    document.getElementById('city').value = selectedClub.clubCity;
    document.getElementById('founded').value = selectedClub.clubFounded;
}

async function getData() {
    await fetch('https://localhost:44374/Club')
        .then(x => x.json())
        .then(y => {
            clubs = y;
            selectedClub = clubs[selectedClubIndex];
            display();
            players();
            club();
            manager();
        }).then(x => isLoading = false);
}

function deleteClub(id) {
    fetch('https://localhost:44374/Club/' + id, {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json', },
        body: null
    })
    .then(response => response)
    .then(data => {
            console.log('Success:', data);
    }).catch((error) => { console.error('Error:', error); });
}

function deletePlayer(id) {
    fetch('https://localhost:44374/Player/' + id, {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json', },
        body: null
    })
    .then(response => response)
    .then(data => {
            console.log('Success:', data);
            players();
    }).catch((error) => { console.error('Error:', error); });
}

function display() {
    document.getElementById('clubs').innerHTML = "";
    const select = document.getElementById("clubs");
    clubs.forEach(t => {
        let option = document.createElement("option");
        option.text = t.clubName;
        option.value = t;
        if (t.clubId == selectedClub.clubId) {
            option.selected = true;
        }
        select.add(option);
    });
}

function playerPosition(position) {
    if (position == 0) {
        return 'Goalkeeper';
    }
    else if (position == 1) {
        return 'Defender';
    }
    else if (position == 2) {
        return 'Midfielder';
    }
    else {
        return 'Forward';
    }
}

function preferredFoot(foot) {
    if (foot == 0) {
        return 'Left';
    }
    else if (foot == 1) {
        return 'Right';
    }
    else {
        return 'Both';
    }
}

function players() {
    document.getElementById('resultarea').innerHTML = "";
    selectedClub.players.sort((a, b) => a.playerPosition - b.playerPosition);
    selectedClub.players.forEach(player => {
        document.getElementById('resultarea').innerHTML +=
            "<tr><td>" + player.playerName + "</td><td>" + player.playerCountry +
            "</td><td>" + new Date(player.playerBirthdate).toLocaleDateString() +
            "</td><td>" + playerPosition(player.playerPosition) +
            "</td><td>" + player.shirtNumber + "</td><td>" + player.height +
            "</td><td>" + preferredFoot(player.preferredFoot) + "</td><td>" + player.playerValue +
            `</td><td><button type="button" onclick="deletePlayer(${player.playerId})">Delete</button></td>`;
    });
}

function club() {
    document.getElementById('club').innerHTML = "";
    document.getElementById('club').innerHTML +=
        '<label style="font-weight: bold;">' + selectedClub.clubName + '</label>' +
        "<label>" + selectedClub.clubColour + "</label>" +
        "<label>" + selectedClub.clubCity + "</label>" +
        "<label>" + selectedClub.clubFounded + "</label>" +
        "<label>" + selectedClub.stadium.stadiumName + "</label>" +
        `<button type="button" onclick="deleteClub(${selectedClub.clubId})">Delete</button>`;
}

function manager() {
    document.getElementById('manager').innerHTML = "";
    document.getElementById('manager').innerHTML +=
        '<label style="font-weight: bold;">' + selectedClub.manager.managerName + '</label>' +
        "<label>" + selectedClub.manager.managerCountry + "</label>" +
        "<label>" + new Date(selectedClub.manager.managerBirthdate).toLocaleDateString() + "</label>" +
        "<label>" + selectedClub.manager.preferredFormation + "</label>" +
        "<label>" + selectedClub.manager.wonChampionship + "</label>" +
        "<label>" + selectedClub.manager.managerStartYear + "</label>";
}

function addPlayer() {
    let ids = clubs.map(x => x.players).flat().map(y => y.playerId);

    let id = 0;
    do {
        id = Math.floor(Math.random() * (999999 - 10000 + 1)) + 10000;
    }
    while (ids.includes(id));

    let name = document.getElementById('name').value;
    let code = document.getElementById('code').value;
    let country = document.getElementById('country').value;
    let birthdate = document.getElementById('birthdate').value;
    let position = parseInt(document.getElementById('position').value);
    let shirtnumber = document.getElementById('shirtnumber').value;
    let height = document.getElementById('height').value;
    let preferredfoot = parseInt(document.getElementById('preferredfoot').value);
    let value = document.getElementById('value').value;

    fetch('https://localhost:44374/Player/' + selectedClub.clubId, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            playerId: id,
            playerName: name,
            countryCode: code,
            playerCountry: country,
            playerBirthdate: birthdate,
            playerPosition: position,
            shirtNumber: shirtnumber,
            height: height,
            preferredFoot: preferredfoot,
            playerValue: value,
            clubId: selectedClub.clubId,
            captain: false
        })
    })
    .then(response => response)
    .then(data => {
        console.log('Success:', data);
        players();
        document.getElementById('name').value = '';
        document.getElementById('code').value = '';
        document.getElementById('country').value = '';
        document.getElementById('birthdate').value = '';
        document.getElementById('position').value = 0;
        document.getElementById('shirtnumber').value = '';
        document.getElementById('height').value = '';
        document.getElementById('preferredfoot').value = 0;
        document.getElementById('value').value = '';
    }).catch((error) => { console.error('Error:', error); });
}

function updateClub() {
    let name = document.getElementById('clubname').value;
    let colour = document.getElementById('colour').value;
    let city = document.getElementById('city').value;
    let founded = document.getElementById('founded').value;

    fetch('https://localhost:44374/Club/' + selectedClub.clubId, {
        method: 'PUT',
        headers: { 'Content-Type': "application/json" },
        body: JSON.stringify({
            clubId: selectedClub.clubId,
            clubName: name,
            clubColour: colour,
            clubCity: city,
            clubFounded: founded,
            stadium: selectedClub.stadium,
            manager: selectedClub.manager,
            players: selectedClub.players,
            users: selectedClub.users
        })
    })
    .then(response => response)
        .then(data => {
        getData();
    }).catch((error) => { console.error('Error:', error); });
}

function loading() {
    if (isLoading == false) {
        document.getElementById('loaded').style.display = "";
        document.getElementById('not_loaded').style.display = "none";
    } else {
        document.getElementById('loaded').style.display = "none";
        document.getElementById('not_loaded').style.display = "";
    }
}