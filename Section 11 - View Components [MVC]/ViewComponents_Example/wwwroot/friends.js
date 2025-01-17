document.querySelector('#load-friends-button').addEventListener('click',async function () {
    const response = await fetch('friends-list')
    const responseBody = await response.text(); //since our response contain plain html code
    document.querySelector('#list').innerHTML = responseBody;
});