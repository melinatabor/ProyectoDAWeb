export const fetchTranslations = async (language) => {
    try {
        const response = await fetch(`api/language/translations/${language}`, {
            method: "GET",
            headers: {
                "Content-Type": "application/json"
            },
        });

        if (response.ok) {
            const data = await response.json();
            return data.traduccion;
        } else {
            console.log(response);
            const errorData = await response.json();
            let msg = errorData.message;
            alert(msg);
        }
    } catch (error) {
        console.error("Error al cargar las traducciones:", error);
    }
};

export const useTranslations = () => {
    const fetchLanguage = async () => {
        try {
            const response = await fetch("api/language/list", {
                method: "GET",
                headers: {
                    "Content-Type": "application/json"
                },
            });

            if (response.ok) {
                const idiomas = await response.json();
                return idiomas.idioma;
            } else {
                console.log(response);
                const errorData = await response.json();
                let msg = errorData.message;
                alert(msg);
            }
        } catch (error) {
            console.error("Error al cargar las traducciones:", error);
        }
    };

    const setLanguage = async (id) => {
        const traducciones = await fetchTranslations(id);
        const idioma = getLanguage()?.idioma;

        if (idioma !== id) {
            sessionStorage.setItem("traducciones", JSON.stringify({ traducciones: traducciones, idioma: id }));
            window.location.reload();
         }
    };

    const getLanguage = () => {
        const traducciones = sessionStorage.getItem("traducciones");
        return JSON.parse(traducciones);
    };

    const clearLanguage = () => {
        sessionStorage.removeItem("traducciones");
    };

    const gettext = (tag) => {
        return getLanguage()?.traducciones.find((traduccion) => traduccion.tag === tag)?.traduccion ?? 'Cargando traducciones...';
    }

    return { fetchLanguage, setLanguage, getLanguage, clearLanguage, gettext };
};