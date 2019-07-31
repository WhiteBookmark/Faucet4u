<template>
    <div v-html="squareBanner"></div>
</template>

<script>
    export default {
        name: "SquareBanner",
        data() {
            return {
                squareBanner: null
            };
        },
        async mounted() {

            try {
                const response = await this.axios.get(process.env.VUE_APP_API_SquareBannerNetworkHTMLCode);
                this.squareBanner = response.data.HTMLCode;
            }
            catch (error) {
                console.log(error);
            }
        }
    };
</script>
