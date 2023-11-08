abstract class tonrich {
  abstract addBadge(): void;

  showSite(url: string, element: HTMLElement, style: string) {
    var iframe = document.getElementById('tonapi-iframe');
    if (iframe) {
      iframe.remove();
    }

    var newIframe = document.createElement('iframe') as HTMLIFrameElement;

    newIframe.classList.add(style);

    newIframe.id = 'tonapi-iframe';
    newIframe.src = url;
    newIframe.setAttribute('scrolling', 'no');
    newIframe.addEventListener('mouseleave', (e) => {
      var iframe = document.getElementById('tonapi-iframe');
      if (iframe) {
        iframe.remove();
      }
    });

    element.appendChild(newIframe);

    document.addEventListener('click', function (event) {
      var iframe = document.getElementById('tonapi-iframe');
      if (iframe && !iframe.contains(event.target as Node)) {
        iframe.remove();
      }
    });
  }
}

export default tonrich;