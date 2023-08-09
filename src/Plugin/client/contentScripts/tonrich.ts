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
      newIframe.addEventListener('mouseleave', (e) => {
        var iframe = document.getElementById('tonapi-iframe');
        if (iframe) {
          iframe.remove();
        }
      });
    
      element.appendChild(newIframe);
    }
}

export default tonrich;